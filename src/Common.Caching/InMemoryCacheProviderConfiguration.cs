﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Common;

using System;

public class InMemoryCacheProviderConfiguration
{
    public TimeSpan? SlidingExpiration { get; set; }

    public DateTimeOffset? AbsoluteExpiration { get; set; }
}