using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperGate.Infra.Migrations
{
    /// <inheritdoc />
    public partial class mig_ChangesInContactWays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnglishTitle",
                table: "ContactWays",
                newName: "EnglishLink");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnglishLink",
                table: "ContactWays",
                newName: "EnglishTitle");
        }
    }
}
