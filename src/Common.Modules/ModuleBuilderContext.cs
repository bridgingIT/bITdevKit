﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;

public class ModuleBuilderContext
{
    public ModuleBuilderContext(IServiceCollection services, IConfiguration configuration = null)
    {
        this.Services = services;
        this.Configuration = configuration;
    }

    public IServiceCollection Services { get; }

    public IConfiguration Configuration { get; }
}