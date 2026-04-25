using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditReviewWithAI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFlagged",
                table: "Reviews",
                newName: "is_suspicious");

            migrationBuilder.AddColumn<string>(
                name: "FraudReasons",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FraudReasons",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "is_suspicious",
                table: "Reviews",
                newName: "IsFlagged");
        }
    }
}
