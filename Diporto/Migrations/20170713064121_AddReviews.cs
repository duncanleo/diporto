using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Diporto.Migrations
{
    public partial class AddReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "place_review",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    author_name = table.Column<string>(nullable: false),
                    author_profile_image_url = table.Column<string>(nullable: true),
                    rating = table.Column<float>(nullable: false),
                    text = table.Column<string>(nullable: false),
                    time = table.Column<DateTime>(nullable: false),
                    place_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_place_review", x => x.id);
                    table.ForeignKey(
                        name: "FK_place_review_place_place_id",
                        column: x => x.place_id,
                        principalTable: "place",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_place_review_place_id",
                table: "place_review",
                column: "place_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "place_review");
        }
    }
}
