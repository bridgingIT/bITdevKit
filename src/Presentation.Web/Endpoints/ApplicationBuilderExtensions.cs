// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace Microsoft.AspNetCore.Builder;

using BridgingIT.DevKit.Common;
using BridgingIT.DevKit.Presentation.Web;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationApplicationExtensions
{
    public static IApplicationBuilder MapApiEndpoints(
        this WebApplication app,
        RouteGroupBuilder routeGroupBuilder = null)
    {
        EnsureArg.IsNotNull(app, nameof(app));

        var endpoints = app.Services.GetService<IEnumerable<IApiEndpoint>>();
        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints.SafeNull())
        {
            endpoint.MapApiEndpoints(builder);
        }

        return app;
    }
}