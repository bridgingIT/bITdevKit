﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Application.Commands;

using System.Threading;
using System.Threading.Tasks;
using BridgingIT.DevKit.Common;
using Humanizer;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly; // TODO: migrate to Polly 8 https://www.pollydocs.org/migration-v8.html
using Polly.Timeout;

public class TimeoutCommandBehavior<TRequest, TResponse> : CommandBehaviorBase<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    public TimeoutCommandBehavior(ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
    }

    protected override bool CanProcess(TRequest request)
    {
        return request is ITimeoutCommand;
    }

    protected override async Task<TResponse> Process(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // timeout only if implements interface
        if (request is not ITimeoutCommand instance)
        {
            return await next().AnyContext();
        }

        var timeoutPolicy = Policy
            .TimeoutAsync(instance.Options.Timeout, TimeoutStrategy.Pessimistic, onTimeoutAsync: async (context, timeout, task) =>
            {
                await Task.Run(() => this.Logger.LogError("{LogKey} command timeout behavior (timeout={Timeout}, type={BehaviorType})", Constants.LogKey, timeout.Humanize(), this.GetType().Name));
            });

        return await timeoutPolicy.ExecuteAsync(async (context) => await next().AnyContext(), cancellationToken);
    }
}