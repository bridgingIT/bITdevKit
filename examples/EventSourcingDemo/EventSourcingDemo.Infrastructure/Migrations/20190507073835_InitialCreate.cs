﻿namespace BridgingIT.DevKit.Examples.EventSourcing.Infrastructure.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            "dbo");

        migrationBuilder.CreateTable(
            "Person",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Firstname = table.Column<string>(nullable: true),
                Lastname = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Person", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Person",
            "EventStore");
    }
}