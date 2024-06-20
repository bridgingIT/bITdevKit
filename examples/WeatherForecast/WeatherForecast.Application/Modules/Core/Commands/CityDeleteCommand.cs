﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Examples.WeatherForecast.Application.Modules.Core;

using System.Linq;
using BridgingIT.DevKit.Application.Commands;
using FluentValidation;
using FluentValidation.Results;

public class CityDeleteCommand(string name) : CommandRequestBase<AggregateDeletedCommandResult>,
    ICacheInvalidateCommand //, IRetryCommand
{
    public string Name { get; } = name;

    CacheInvalidateCommandOptions ICacheInvalidateCommand.Options => new() { Key = "application_" };

    //RetryCommandOptions IRetryCommand.Options => new() { Attempts = 3, Backoff = new TimeSpan(0, 0, 0, 1) };

    public override ValidationResult Validate() =>
        new Validator().Validate(this);

    public class Validator : AbstractValidator<CityDeleteCommand>
    {
        public Validator()
        {
            this.RuleFor(c => c.Name).NotNull().NotEmpty().Length(3, 128);
            this.RuleFor(c => c.Name).Must(this.ContainLettersOnly).WithMessage("Only letters allowed.");
        }

        private bool ContainLettersOnly(string arg)
        {
            return arg.All(char.IsLetter);
        }
    }
}
