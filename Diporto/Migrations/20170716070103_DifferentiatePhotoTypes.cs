using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diporto.Migrations
{
    public partial class DifferentiatePhotoTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "url",
                table: "place_photo");

            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "place_photo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "google_place_id",
                table: "place_photo",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_google_places_image",
                table: "place_photo",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_name",
                table: "place_photo");

            migrationBuilder.DropColumn(
                name: "google_place_id",
                table: "place_photo");

            migrationBuilder.DropColumn(
                name: "is_google_places_image",
                table: "place_photo");

            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "place_photo",
                nullable: false,
                defaultValue: "");
        }
    }
}
