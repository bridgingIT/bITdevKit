﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Application.Entities;

using System;
using BridgingIT.DevKit.Application.Commands;
using BridgingIT.DevKit.Common;
using BridgingIT.DevKit.Domain.Model;
using FluentValidation;
using FluentValidation.Results;

public abstract class EntityUpdateCommandBase<TEntity>(TEntity entity, string identity = null) :
    CommandRequestBase<Result<EntityUpdatedCommandResult>>,
    IEntityUpdateCommand<TEntity>
    where TEntity : class, IEntity
{
    private List<AbstractValidator<EntityUpdateCommandBase<TEntity>>> validators;

    public TEntity Entity { get; } = entity;

    object IEntityUpdateCommand.Entity
    {
        get { return this.Entity; }
    }

    public string Identity { get; } = identity;

    public EntityUpdateCommandBase<TEntity> AddValidator(
        AbstractValidator<EntityUpdateCommandBase<TEntity>> validator)
    {
        (this.validators ??= new()).AddOrUpdate(validator);

        return this;
    }

    public EntityUpdateCommandBase<TEntity> AddValidator<TValidator>()
        where TValidator : class => this.AddValidator(
            Factory<TValidator>.Create() as AbstractValidator<EntityUpdateCommandBase<TEntity>>);

    public override ValidationResult Validate() =>
        new Validator(this.validators).Validate(this);

    public class Validator : AbstractValidator<EntityUpdateCommandBase<TEntity>>
    {
        public Validator(IEnumerable<AbstractValidator<EntityUpdateCommandBase<TEntity>>> validators = null)
        {
            foreach (var validator in validators.SafeNull())
            {
                this.Include(validator); // https://docs.fluentvalidation.net/en/latest/including-rules.html
            }

            this.RuleFor(c => c.Entity).NotNull().NotEmpty().ChildRules(c =>
            {
                c.RuleFor(c => c.Id).Must(id => id.To<Guid>() != Guid.Empty).WithMessage("Invalid guid.");
                // TODO: fluentvalidator message localization https://docs.fluentvalidation.net/en/latest/localization.html
            });
        }
    }
}