using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PublishDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PublishDate",
                table: "Posts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: DateTimeOffset.UtcNow);

            migrationBuilder.AddColumn<byte>(
                name: "VisibilityType",
                table: "Posts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "VisibilityType",
                table: "Posts");
        }
    }
}
