using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperGate.Infra.Migrations
{
    /// <inheritdoc />
    public partial class mig_EditedPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnglishContent",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishContent",
                table: "Posts");
        }
    }
}
