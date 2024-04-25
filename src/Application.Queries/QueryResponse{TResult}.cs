﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Application.Queries;
public class QueryResponse<TResult>
{
    public QueryResponse(string cancelledReason = null)
    {
        if (!string.IsNullOrEmpty(cancelledReason))
        {
            this.Cancelled = true;
            this.CancelledReason = cancelledReason;
        }
    }

    public bool Cancelled { get; private set; }

    public string CancelledReason { get; private set; }

    public TResult Result { get; set; }

    public void SetCancelled(string cancelledReason)
    {
        if (!string.IsNullOrEmpty(cancelledReason))
        {
            this.Cancelled = true;
            this.CancelledReason = cancelledReason;
        }
    }
}
