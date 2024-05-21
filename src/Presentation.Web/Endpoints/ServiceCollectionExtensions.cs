// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using BridgingIT.DevKit.Common;
using BridgingIT.DevKit.Presentation.Web;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiEndpoints(
        this IServiceCollection services)
    {
        EnsureArg.IsNotNull(services, nameof(services));

        return services.AddApiEndpoints(
            BridgingIT.DevKit.Common.AssemblyExtensions
                .SafeGetTypes<IApiEndpoints>(AppDomain.CurrentDomain.GetAssemblies())
                    .Select(t => t.Assembly).Distinct());
    }

    public static IServiceCollection AddApiEndpoints<T>(
        this IServiceCollection services)
        where T : IApiEndpoints
    {
        EnsureArg.IsNotNull(services, nameof(services));

        return services.AddApiEndpoints(new[] { typeof(T).Assembly });
    }

    public static IServiceCollection AddApiEndpoints(
        this IServiceCollection services,
        Assembly assembly)
    {
        EnsureArg.IsNotNull(services, nameof(services));

        return services.AddApiEndpoints(new[] { assembly });
    }

    public static IServiceCollection AddApiEndpoints(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        EnsureArg.IsNotNull(services, nameof(services));

        foreach (var assembly in assemblies.SafeNull())
        {
            var serviceDescriptors = assembly.SafeGetTypes<IApiEndpoints>()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => ServiceDescriptor.Transient(typeof(IApiEndpoints), t))
                .ToArray();

            if (serviceDescriptors.SafeAny())
            {
                services.TryAddEnumerable(serviceDescriptors);

                foreach (var serviceDescriptor in serviceDescriptors)
                {
                    Log.Logger.Information("{LogKey} api endpoints added (type={ApiEndpointsType})", "REQ", serviceDescriptor.ImplementationType.Name);
                }
            }
        }

        return services;
    }
}