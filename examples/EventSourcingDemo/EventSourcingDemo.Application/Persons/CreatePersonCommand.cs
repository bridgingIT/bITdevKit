﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Examples.EventSourcingDemo.Application.Persons;

using BridgingIT.DevKit.Application.Commands;

public class CreatePersonCommand : CommandRequestBase<PersonOverviewViewModel>
{
    public CreatePersonViewModel Model { get; set; }
}