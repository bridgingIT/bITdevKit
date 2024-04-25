﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Application.JobScheduling;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class JobSchedulingBuilderContext
{
    public JobSchedulingBuilderContext(IServiceCollection services, IConfiguration configuration = null, JobSchedulingOptions options = null)
    {
        this.Services = services;
        this.Configuration = configuration;
        this.Options = options;
    }

    public IServiceCollection Services { get; }

    public IConfiguration Configuration { get; }

    public JobSchedulingOptions Options { get; }
}