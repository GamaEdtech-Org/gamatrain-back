using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TicketReceivers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_ApplicationUsers_CreationUserId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "CreationUserId",
                table: "Tickets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_CreationUserId",
                table: "Tickets",
                newName: "IX_Tickets_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Receivers",
                table: "Tickets",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Receivers",
                table: "TicketReplies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_ApplicationUsers_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_ApplicationUsers_UserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Receivers",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Receivers",
                table: "TicketReplies");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Tickets",
                newName: "CreationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                newName: "IX_Tickets_CreationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_ApplicationUsers_CreationUserId",
                table: "Tickets",
                column: "CreationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }
    }
}
