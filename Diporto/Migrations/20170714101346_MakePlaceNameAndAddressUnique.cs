using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diporto.Migrations
{
    public partial class MakePlaceNameAndAddressUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_place_name_address",
                table: "place",
                columns: new[] { "name", "address" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_place_name_address",
                table: "place");
        }
    }
}
