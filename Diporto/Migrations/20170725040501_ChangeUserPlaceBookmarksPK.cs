using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Diporto.Migrations
{
    public partial class ChangeUserPlaceBookmarksPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_place_bookmark",
                table: "user_place_bookmark");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "user_place_bookmark",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_place_bookmark",
                table: "user_place_bookmark",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_user_place_bookmark_place_id_user_id",
                table: "user_place_bookmark",
                columns: new[] { "place_id", "user_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_place_bookmark",
                table: "user_place_bookmark");

            migrationBuilder.DropIndex(
                name: "IX_user_place_bookmark_place_id_user_id",
                table: "user_place_bookmark");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "user_place_bookmark",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_place_bookmark",
                table: "user_place_bookmark",
                columns: new[] { "place_id", "user_id" });
        }
    }
}
