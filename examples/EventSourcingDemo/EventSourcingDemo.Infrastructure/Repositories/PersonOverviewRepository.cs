﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Examples.EventSourcingDemo.Infrastructure.Repositories;

using BridgingIT.DevKit.Common;
using BridgingIT.DevKit.Infrastructure.EntityFramework.Repositories;
using Domain.Model;
using Domain.Repositories;
using Models;

public class PersonOverviewRepository : EntityFrameworkGenericRepository<PersonOverview, PersonDatabaseEntity>,
    IPersonOverviewRepository
{
    public PersonOverviewRepository(Builder<EntityFrameworkRepositoryOptionsBuilder, EntityFrameworkRepositoryOptions> optionsBuilder)
        : base(optionsBuilder)
    {
    }
}