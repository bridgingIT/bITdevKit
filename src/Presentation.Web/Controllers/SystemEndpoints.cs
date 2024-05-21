// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Presentation.Web;

using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using BridgingIT.DevKit.Application.Queries;
using BridgingIT.DevKit.Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

public class SystemEndpoints : ApiEndpointsBase
{
    public override void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/_system/echo", async (IMediator mediator) =>
        {
            var response = await mediator.Send(new EchoQuery());
            return response.Result;
        }).Produces<string>(200)
          .Produces<ProblemDetails>(500)
          .WithTags("_system/echo");

        app.MapGet("/api/_system/info", async (HttpContext httpContext) =>
        {
            return new SystemInfo
            {
                Request = new Dictionary<string, object>
                {
                    ["isLocal"] = IsLocal(httpContext?.Request),
                    ["host"] = Dns.GetHostName(),
                    ["ip"] = (await Dns.GetHostAddressesAsync(Dns.GetHostName())).Select(i => i.ToString()).Where(i => i.Contains('.')),
                },
                Runtime = new Dictionary<string, string>
                {
                    ["name"] = Assembly.GetEntryAssembly().GetName().Name,
                    ["environment"] = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                    ["version"] = Assembly.GetEntryAssembly().GetName().Version.ToString(),
                    ["versionInformation"] = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion,
                    ["buildDate"] = GetBuildDate(Assembly.GetEntryAssembly()).ToString("o"),
                    ["processName"] = Process.GetCurrentProcess().ProcessName.Equals("dotnet", StringComparison.InvariantCultureIgnoreCase) ? $"{Process.GetCurrentProcess().ProcessName} (kestrel)" : Process.GetCurrentProcess().ProcessName,
                    ["process64Bits"] = Environment.Is64BitProcess.ToString(),
                    ["framework"] = RuntimeInformation.FrameworkDescription,
                    ["runtime"] = RuntimeInformation.RuntimeIdentifier,
                    ["machineName"] = Environment.MachineName,
                    ["processorCount"] = Environment.ProcessorCount.ToString(),
                    ["osDescription"] = RuntimeInformation.OSDescription,
                    ["osArchitecture"] = RuntimeInformation.OSArchitecture.ToString()
                }
            };
        })
           .Produces<SystemInfo>(200)
           .Produces<ProblemDetails>(500)
           .WithTags("_system/info");

        app.MapGet("/api/_system/modules", (IEnumerable<IModule> modules) =>
            Task.FromResult(modules))
            .Produces<IEnumerable<IModule>>(200)
            .Produces<ProblemDetails>(500)
            .WithTags("_system/modules");
    }

    private static bool IsLocal(HttpRequest source)
    {
        // https://stackoverflow.com/a/41242493/7860424
        var connection = source?.HttpContext?.Connection;
        if (IsIpAddressSet(connection?.RemoteIpAddress))
        {
            return IsIpAddressSet(connection.LocalIpAddress)
                //if local is same as remote, then we are local
                ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                //else we are remote if the remote IP address is not a loopback address
                : IPAddress.IsLoopback(connection.RemoteIpAddress);
        }

        return true;

        static bool IsIpAddressSet(IPAddress address)
        {
            return address is not null && address.ToString() != "::1";
        }
    }

    private static DateTime GetBuildDate(Assembly assembly)
    {
        // origin: https://www.meziantou.net/2018/09/24/getting-the-date-of-build-of-a-net-assembly-at-runtime
        // note: project file needs to contain:
        //       <PropertyGroup><SourceRevisionId>build$([System.DateTime]::UtcNow.ToString("yyyyMMddHHmmss"))</SourceRevisionId></PropertyGroup>
        const string BuildVersionMetadataPrefix1 = "+build";
        const string BuildVersionMetadataPrefix2 = ".build"; // TODO: make this an array of allowable prefixes
        var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (attribute?.InformationalVersion is not null)
        {
            var value = attribute.InformationalVersion;
            var prefix = BuildVersionMetadataPrefix1;
            var index = value.IndexOf(BuildVersionMetadataPrefix1, StringComparison.OrdinalIgnoreCase);
            // fallback for '.build' prefix
            if (index == -1)
            {
                prefix = BuildVersionMetadataPrefix2;
                index = value.IndexOf(BuildVersionMetadataPrefix2, StringComparison.OrdinalIgnoreCase);
            }

            if (index > 0)
            {
                value = value[(index + prefix.Length)..];
                if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                {
                    return result;
                }
            }
        }

        return default;
    }
}