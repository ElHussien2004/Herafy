using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditActiveUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Technicians_TechnicianId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "TechnicianId",
                table: "Documents",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "DocumentUserId",
                table: "Technicians",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Technicians",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentUserId",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_Documents_DocumentUserId",
                table: "Technicians",
                column: "DocumentUserId",
                principalTable: "Documents",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Documents_DocumentUserId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents");

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
                name: "State",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "DocumentUserId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Documents",
                newName: "TechnicianId");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Technicians",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Technicians_TechnicianId",
                table: "Documents",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
