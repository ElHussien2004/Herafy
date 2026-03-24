using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class editPKClientandTechnical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Clients_ClientId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Technicians_TechnicianId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Technicians_TechnicianId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Technicians_TechnicianId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Technicians",
                table: "Technicians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Technicians",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Technicians",
                table: "Technicians",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Clients_ClientId",
                table: "Chats",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Technicians_TechnicianId",
                table: "Chats",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Technicians_TechnicianId",
                table: "Documents",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Technicians_TechnicianId",
                table: "Orders",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Clients_ClientId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Technicians_TechnicianId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Technicians_TechnicianId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Technicians_TechnicianId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Technicians",
                table: "Technicians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Technicians",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Technicians",
                table: "Technicians",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Clients_ClientId",
                table: "Chats",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Technicians_TechnicianId",
                table: "Chats",
                column: "TechnicianId",
                principalTable: "Technicians",
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
                name: "FK_Orders_Clients_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Technicians_TechnicianId",
                table: "Orders",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
