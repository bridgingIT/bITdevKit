﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Application.Messaging;

using BridgingIT.DevKit.Common;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

public class ModuleScopeMessageHandlerBehavior(
    ILoggerFactory loggerFactory,
    IEnumerable<IModuleContextAccessor> moduleAccessors = null,
    IEnumerable<ActivitySource> activitySources = null) : MessageHandlerBehaviorBase(loggerFactory)
{
    private readonly IEnumerable<IModuleContextAccessor> moduleAccessors = moduleAccessors;
    private readonly IEnumerable<ActivitySource> activitySources = activitySources;

    public override async Task Handle<TMessage>(TMessage message, CancellationToken cancellationToken, object handler, MessageHandlerDelegate next)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var moduleNameOrigin = message?.Properties?.GetValue(ModuleConstants.ModuleNameOriginKey)?.ToString().EmptyToNull() ?? ModuleConstants.UnknownModuleName;
        var correlationId = message?.Properties?.GetValue(Constants.CorrelationIdKey)?.ToString();
        var flowId = message?.Properties?.GetValue(Constants.FlowIdKey)?.ToString();
        var parentId = message?.Properties?.GetValue(ModuleConstants.ActivityParentIdKey)?.ToString();

        var module = this.moduleAccessors.Find(message?.GetType());
        var moduleName = module?.Name ?? ModuleConstants.UnknownModuleName;

        using (this.Logger.BeginScope(new Dictionary<string, object>
        {
            [ModuleConstants.ModuleNameOriginKey] = moduleNameOrigin,
            [ModuleConstants.ModuleNameKey] = moduleName
        }))
        {
            if (module is not null && !module.Enabled)
            {
                throw new ModuleNotEnabledException(module.Name);
            }
            else
            {
                var messageType = message.GetType().PrettyName(false);

                if (!string.IsNullOrEmpty(moduleNameOrigin) && !moduleNameOrigin.Equals(module?.Name, StringComparison.OrdinalIgnoreCase))
                {
                    await this.activitySources.Find(module.Name).StartActvity(
                        $"MODULE {module.Name}",
                        async (a, c) => await this.activitySources.Find(module?.Name).StartActvity(
                            $"MESSAGE_PROCESS {messageType}",
                            async (a, c) => await next().AnyContext(),
                            kind: ActivityKind.Consumer,
                            tags: new Dictionary<string, string> { ["messaging.module.origin"] = moduleNameOrigin, ["messaging.message_id"] = message.Id, ["messaging.message_type"] = messageType },
                            cancellationToken: c),
                        parentId: parentId,
                        baggages: new Dictionary<string, string> { [ActivityConstants.ModuleNameTagKey] = moduleName, [ActivityConstants.CorrelationIdTagKey] = correlationId, [ActivityConstants.FlowIdTagKey] = flowId });
                }
                else
                {
                    await this.activitySources.Find(module?.Name).StartActvity(
                        $"MESSAGE_PROCESS {messageType}",
                        async (a, c) => await next().AnyContext(),
                        kind: ActivityKind.Consumer,
                        parentId: parentId,
                        tags: new Dictionary<string, string> { ["messaging.module.origin"] = moduleNameOrigin, ["messaging.message_id"] = message.Id, ["messaging.message_type"] = messageType },
                        baggages: new Dictionary<string, string> { [ActivityConstants.ModuleNameTagKey] = moduleName, [ActivityConstants.CorrelationIdTagKey] = correlationId, [ActivityConstants.FlowIdTagKey] = flowId });
                }
            }
        }
    }
}