using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditeUserIdtoIDfromBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Technicians_TechnicianId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Users_UserId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Technicians_TechnicianId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Technicians_TechnicianId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_Users_UserId",
                table: "Technicians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Technicians",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_UserId",
                table: "Technicians");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Technicians");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Clients",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_UserId",
                table: "Clients",
                newName: "IX_Clients_Id");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Technicians",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Technicians",
                table: "Technicians",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_Id",
                table: "Technicians",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Technicians_TechnicianId",
                table: "Chats",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Users_Id",
                table: "Clients",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Technicians_TechnicianId",
                table: "Documents",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Technicians_TechnicianId",
                table: "Orders",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_Users_Id",
                table: "Technicians",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Technicians_TechnicianId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Users_Id",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Technicians_TechnicianId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Technicians_TechnicianId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_Users_Id",
                table: "Technicians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Technicians",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_Id",
                table: "Technicians");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Clients",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_Id",
                table: "Clients",
                newName: "IX_Clients_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Technicians",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Technicians",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Technicians",
                table: "Technicians",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_UserId",
                table: "Technicians",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Technicians_TechnicianId",
                table: "Chats",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Users_UserId",
                table: "Clients",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Technicians_TechnicianId",
                table: "Documents",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Technicians_TechnicianId",
                table: "Orders",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_Users_UserId",
                table: "Technicians",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
