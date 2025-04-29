using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PreviousTransactionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PreviousTransactionId",
                table: "Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PreviousTransactionId",
                table: "Transactions",
                column: "PreviousTransactionId",
                unique: true,
                filter: "[PreviousTransactionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Transactions_PreviousTransactionId",
                table: "Transactions",
                column: "PreviousTransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Transactions_PreviousTransactionId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PreviousTransactionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PreviousTransactionId",
                table: "Transactions");
        }
    }
}
