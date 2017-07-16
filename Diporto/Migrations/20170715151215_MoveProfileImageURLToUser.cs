using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diporto.Migrations
{
    public partial class MoveProfileImageURLToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_place_review_place_place_id",
                table: "place_review");

            migrationBuilder.DropColumn(
                name: "author_name",
                table: "place_review");

            migrationBuilder.DropColumn(
                name: "author_profile_image_url",
                table: "place_review");

            migrationBuilder.AddColumn<string>(
                name: "profile_image_url",
                table: "user",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "place_id",
                table: "place_review",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_place_review_place_place_id",
                table: "place_review",
                column: "place_id",
                principalTable: "place",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_place_review_place_place_id",
                table: "place_review");

            migrationBuilder.DropColumn(
                name: "profile_image_url",
                table: "user");

            migrationBuilder.AlterColumn<int>(
                name: "place_id",
                table: "place_review",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "author_name",
                table: "place_review",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "author_profile_image_url",
                table: "place_review",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_place_review_place_place_id",
                table: "place_review",
                column: "place_id",
                principalTable: "place",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
