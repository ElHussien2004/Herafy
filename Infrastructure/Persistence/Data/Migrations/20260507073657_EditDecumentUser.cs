using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditDecumentUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Documents_DocumentUserId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_Documents_DocumentUserId",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_DocumentUserId",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Clients_DocumentUserId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DocumentUserId",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "DocumentUserId",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentUserId",
                table: "Technicians",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentUserId",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_DocumentUserId",
                table: "Technicians",
                column: "DocumentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_DocumentUserId",
                table: "Clients",
                column: "DocumentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Documents_DocumentUserId",
                table: "Clients",
                column: "DocumentUserId",
                principalTable: "Documents",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_Documents_DocumentUserId",
                table: "Technicians",
                column: "DocumentUserId",
                principalTable: "Documents",
                principalColumn: "UserId");
        }
    }
}
