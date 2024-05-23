// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Presentation.Web;

public class SystemEndpointsOptions : EndpointsOptionsBase
{
    public SystemEndpointsOptions()
    {
        this.GroupPrefix = "/api/_system";
        this.GroupTag = "_system";
    }
}

public abstract class EndpointsOptionsBase
{
    public string GroupPrefix { get; set; } = "/api";

    public string GroupTag { get; set; } = string.Empty;

    public bool RequireAuthorization { get; set; }
}