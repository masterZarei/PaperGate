using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperGate.Infra.Migrations
{
    /// <inheritdoc />
    public partial class mig_UsefulLinksAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Categories_CategoryInfoId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CategoryInfoId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CategoryInfoId",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "UsefulLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsefulLinks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Categories_CategoryId",
                table: "Posts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Categories_CategoryId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "UsefulLinks");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "CategoryInfoId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryInfoId",
                table: "Posts",
                column: "CategoryInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Categories_CategoryInfoId",
                table: "Posts",
                column: "CategoryInfoId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
