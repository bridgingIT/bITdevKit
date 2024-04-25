﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Domain;

using System;

public class InvalidEntityIdException : Exception
{
    public InvalidEntityIdException()
    {
    }

    public InvalidEntityIdException(string message)
        : base(message)
    {
    }

    public InvalidEntityIdException(string message, Exception innerException)
    : base(message, innerException)
    {
    }
}
