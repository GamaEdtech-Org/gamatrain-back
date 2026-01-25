using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVotingPower : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VotingPowers_ApplicationUsers_CreationUserId",
                table: "VotingPowers");

            migrationBuilder.DropForeignKey(
                name: "FK_VotingPowers_ApplicationUsers_LastModifyUserId",
                table: "VotingPowers");

            migrationBuilder.DropIndex(
                name: "IX_VotingPowers_CreationUserId",
                table: "VotingPowers");

            migrationBuilder.DropIndex(
                name: "IX_VotingPowers_LastModifyUserId",
                table: "VotingPowers");

            migrationBuilder.DropColumn(
                name: "CreationUserId",
                table: "VotingPowers");

            migrationBuilder.DropColumn(
                name: "LastModifyDate",
                table: "VotingPowers");

            migrationBuilder.DropColumn(
                name: "LastModifyUserId",
                table: "VotingPowers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreationUserId",
                table: "VotingPowers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifyDate",
                table: "VotingPowers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifyUserId",
                table: "VotingPowers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VotingPowers_CreationUserId",
                table: "VotingPowers",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingPowers_LastModifyUserId",
                table: "VotingPowers",
                column: "LastModifyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VotingPowers_ApplicationUsers_CreationUserId",
                table: "VotingPowers",
                column: "CreationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VotingPowers_ApplicationUsers_LastModifyUserId",
                table: "VotingPowers",
                column: "LastModifyUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }
    }
}
