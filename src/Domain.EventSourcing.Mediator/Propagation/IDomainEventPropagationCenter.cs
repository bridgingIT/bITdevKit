﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Domain.EventSourcing;

using BridgingIT.DevKit.Domain.EventSourcing.Model;

public interface IDomainEventPropagationCenter
{
    void ApplyDomainEvent<TAggregate>(DomainEventWithGuid domainEvent, TAggregate aggregate)
        where TAggregate : IAggregateRootWithGuid, new();
}