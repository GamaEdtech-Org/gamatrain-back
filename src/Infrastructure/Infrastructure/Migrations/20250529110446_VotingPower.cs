using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VotingPower : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VotingPowers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProposalId = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(36,18)", precision: 36, scale: 18, nullable: false),
                    TokenAccount = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: false),
                    LastModifyDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastModifyUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingPowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VotingPowers_ApplicationUsers_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VotingPowers_ApplicationUsers_LastModifyUserId",
                        column: x => x.LastModifyUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VotingPowers_CreationUserId",
                table: "VotingPowers",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingPowers_LastModifyUserId",
                table: "VotingPowers",
                column: "LastModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingPowers_ProposalId",
                table: "VotingPowers",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingPowers_WalletAddress",
                table: "VotingPowers",
                column: "WalletAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VotingPowers");
        }
    }
}
