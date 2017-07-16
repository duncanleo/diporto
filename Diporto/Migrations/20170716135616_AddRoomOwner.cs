using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diporto.Migrations
{
    public partial class AddRoomOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "room",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "owner_id",
                table: "room",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_room_owner_id",
                table: "room",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "FK_room_user_owner_id",
                table: "room",
                column: "owner_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_room_user_owner_id",
                table: "room");

            migrationBuilder.DropIndex(
                name: "IX_room_owner_id",
                table: "room");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "room");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "room",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
