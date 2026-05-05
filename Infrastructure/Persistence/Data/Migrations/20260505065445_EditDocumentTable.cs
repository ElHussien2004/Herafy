using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditDocumentTable : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "DocumentUserId",
                table: "Technicians",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentUserId",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Documents_DocumentUserId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_Documents_DocumentUserId",
                table: "Technicians");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentUserId",
                table: "Technicians",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DocumentUserId",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Documents_DocumentUserId",
                table: "Clients",
                column: "DocumentUserId",
                principalTable: "Documents",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_Documents_DocumentUserId",
                table: "Technicians",
                column: "DocumentUserId",
                principalTable: "Documents",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
