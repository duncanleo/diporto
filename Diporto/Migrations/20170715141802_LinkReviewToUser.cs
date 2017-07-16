using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diporto.Migrations
{
    public partial class LinkReviewToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:cube", "'cube', '', ''")
                .Annotation("Npgsql:PostgresExtension:earthdistance", "'earthdistance', '', ''")
                .Annotation("Npgsql:PostgresExtension:postgis", "'postgis', '', ''");

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "place_review",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_place_review_user_id",
                table: "place_review",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_place_review_user_user_id",
                table: "place_review",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_place_review_user_user_id",
                table: "place_review");

            migrationBuilder.DropIndex(
                name: "IX_place_review_user_id",
                table: "place_review");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "place_review");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:cube", "'cube', '', ''")
                .OldAnnotation("Npgsql:PostgresExtension:earthdistance", "'earthdistance', '', ''")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", "'postgis', '', ''");
        }
    }
}
