﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Application.Messaging;

public class InProcessMessageBrokerConfiguration
{
    public int ProcessDelay { get; set; }

    public TimeSpan? MessageExpiration { get; set; }
}