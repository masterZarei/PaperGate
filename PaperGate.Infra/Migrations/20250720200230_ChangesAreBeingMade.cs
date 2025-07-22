using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperGate.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ChangesAreBeingMade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaperCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryInfoId",
                table: "Papers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowOnSlider",
                table: "Papers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Papers_CategoryInfoId",
                table: "Papers",
                column: "CategoryInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Papers_Categories_CategoryInfoId",
                table: "Papers",
                column: "CategoryInfoId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Papers_Categories_CategoryInfoId",
                table: "Papers");

            migrationBuilder.DropIndex(
                name: "IX_Papers_CategoryInfoId",
                table: "Papers");

            migrationBuilder.DropColumn(
                name: "CategoryInfoId",
                table: "Papers");

            migrationBuilder.DropColumn(
                name: "ShowOnSlider",
                table: "Papers");

            migrationBuilder.CreateTable(
                name: "PaperCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PaperId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaperCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaperCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaperCategories_Papers_PaperId",
                        column: x => x.PaperId,
                        principalTable: "Papers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaperCategories_CategoryId",
                table: "PaperCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaperCategories_PaperId",
                table: "PaperCategories",
                column: "PaperId");
        }
    }
}
