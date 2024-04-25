﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Examples.DinnerFiesta.Modules.Core.UnitTests.Application;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BridgingIT.DevKit.Domain.Repositories;
using BridgingIT.DevKit.Examples.DinnerFiesta.Modules.Core.Application;
using BridgingIT.DevKit.Examples.DinnerFiesta.Modules.Core.Domain;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

public class MenuCreateCommandHandlerTests
{
    [Fact]
    public async Task Process_ValidCommand_ReturnsResponse()
    {
        // Arrange
        var ticks = DateTime.UtcNow.Ticks;
        var loggerFactory = Substitute.For<ILoggerFactory>();
        var hostId = Stubs.Hosts(ticks).ToArray()[1].Id.Value; // Erik
        var menu = Stubs.Menus(ticks).ToArray()[0]; // Vegetarian Delights
        var repository = Substitute.For<IGenericRepository<Menu>>();
        repository.InsertAsync(
            Arg.Any<Menu>(),
            Arg.Any<CancellationToken>()).Returns(menu);
        var command = CreateCommand(menu, hostId);

        // Act
        var sut = new MenuCreateCommandHandler(loggerFactory, repository);
        var response = await sut.Process(command, CancellationToken.None);

        // Assert
        response.ShouldNotBeNull();
        response.Result.ShouldNotBeNull();
        response.Result.IsSuccess.ShouldBeTrue();
        response.Result.Value.ShouldNotBeNull();
        response.Result.Value.Name.ShouldBe(menu.Name);
        await repository.Received(1).InsertAsync(Arg.Any<Menu>(), Arg.Any<CancellationToken>());
    }

    //[Fact]
    //public async Task Process_NullCommand_ThrowsArgumentNullException()
    //{
    //    // Arrange
    //    var loggerFactory = Substitute.For<ILoggerFactory>();
    //    var repository = Substitute.For<IGenericRepository<Menu>>();
    //    var handler = new MenuCreateCommandHandler(loggerFactory, repository);

    //    // Act & Assert
    //    TODO: refactor to use Shouldly
    //    await Assert.ThrowsAsync<ArgumentNullException>(async () => await handler.Process(null, CancellationToken.None));
    //}

    private static MenuCreateCommand CreateCommand(Menu menu, Guid hostId)
    {
        return new MenuCreateCommand()
        {
            Name = menu.Name,
            Description = menu.Description,
            HostId = hostId.ToString(),
            Sections = menu.Sections.Select(s =>
                new MenuCreateCommand.MenuSection
                {
                    Name = s.Name,
                    Description = s.Description,
                    Items = s.Items.Select(i => new MenuCreateCommand.MenuSectionItem { Name = i.Name, Description = i.Description }).ToList()
                }).ToList()
        };
    }
}