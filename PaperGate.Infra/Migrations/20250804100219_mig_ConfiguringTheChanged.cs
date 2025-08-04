using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperGate.Infra.Migrations
{
    /// <inheritdoc />
    public partial class mig_ConfiguringTheChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaperKeywords_Papers_PaperId",
                table: "PaperKeywords");

            migrationBuilder.DropForeignKey(
                name: "FK_Papers_AspNetUsers_AuthorId",
                table: "Papers");

            migrationBuilder.DropForeignKey(
                name: "FK_Papers_Categories_CategoryInfoId",
                table: "Papers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Papers",
                table: "Papers");

            migrationBuilder.RenameTable(
                name: "Papers",
                newName: "Posts");

            migrationBuilder.RenameColumn(
                name: "PaperId",
                table: "PaperKeywords",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PaperKeywords_PaperId",
                table: "PaperKeywords",
                newName: "IX_PaperKeywords_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Papers_CategoryInfoId",
                table: "Posts",
                newName: "IX_Posts_CategoryInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Papers_AuthorId",
                table: "Posts",
                newName: "IX_Posts_AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "ContactWays",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaperKeywords_Posts_PostId",
                table: "PaperKeywords",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId",
                table: "Posts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Categories_CategoryInfoId",
                table: "Posts",
                column: "CategoryInfoId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaperKeywords_Posts_PostId",
                table: "PaperKeywords");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Categories_CategoryInfoId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Papers");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "PaperKeywords",
                newName: "PaperId");

            migrationBuilder.RenameIndex(
                name: "IX_PaperKeywords_PostId",
                table: "PaperKeywords",
                newName: "IX_PaperKeywords_PaperId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CategoryInfoId",
                table: "Papers",
                newName: "IX_Papers_CategoryInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_AuthorId",
                table: "Papers",
                newName: "IX_Papers_AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "ContactWays",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Papers",
                table: "Papers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaperKeywords_Papers_PaperId",
                table: "PaperKeywords",
                column: "PaperId",
                principalTable: "Papers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Papers_AspNetUsers_AuthorId",
                table: "Papers",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Papers_Categories_CategoryInfoId",
                table: "Papers",
                column: "CategoryInfoId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
