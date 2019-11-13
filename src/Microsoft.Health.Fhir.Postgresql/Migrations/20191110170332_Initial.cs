// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.Health.Fhir.Postgresql.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Resource = table.Column<string>(type: "jsonb", nullable: true),
                    ResourceId = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ResourceType = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedClaims = table.Column<List<string>>(nullable: true),
                    Version = table.Column<double>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ResourceId_Version",
                table: "Resources",
                columns: new[] { "ResourceId", "Version" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resources");
        }
    }
}
