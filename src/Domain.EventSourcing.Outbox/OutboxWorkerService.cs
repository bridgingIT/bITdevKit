﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Domain.EventSourcing.Outbox;

using System;
using System.Threading.Tasks;
using BridgingIT.DevKit.Common;
using BridgingIT.DevKit.Common.Options;
using BridgingIT.DevKit.Domain.EventSourcing.AggregatePublish;
using BridgingIT.DevKit.Domain.Outbox;
using BridgingIT.DevKit.Domain.Repositories;
using BridgingIT.DevKit.Domain.Specifications;
using Microsoft.Extensions.Logging;

public class OutboxWorkerService : IOutboxWorkerService
{
    private readonly IOutboxMessageWorkerRepository repository;
    private readonly IAggregateEventOutboxReceiver receiver;
    private readonly ILogger<OutboxWorkerService> logger;

    public OutboxWorkerService(
        IOutboxMessageWorkerRepository repository,
        IAggregateEventOutboxReceiver receiver,
        ILoggerOptions loggerOptions)
    {
        this.repository = repository;
        this.receiver = receiver;
        this.logger = loggerOptions.CreateLogger<OutboxWorkerService>();
    }

    public async Task DoWorkAsync()
    {
        foreach (var message in await this.repository.FindAllAsync(
            new Specification<OutboxMessage>(m => !m.IsProcessed),
            new FindOptions<OutboxMessage>(0, 0,
                new OrderOption<OutboxMessage>(o => o.TimeStamp))).AnyContext())
        {
            try
            {
                var result = await this.receiver.ReceiveAndPublishAsync(message).AnyContext();
                if (result.projectionSended && result.eventOccuredNotified && result.eventOccuredSended)
                {
                    message.IsProcessed = true;
                }
                else
                {
                    message.RetryAttempt++;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error processing message {message.Id}. The error was: {ex.Message}");
                message.RetryAttempt++;
            }

            await this.repository.UpdateAsync(message).AnyContext();
        }
    }
}