using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SchoolImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifyDate",
                table: "SchoolComments",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifyUserId",
                table: "SchoolComments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "SchoolComments",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "SchoolImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    FileType = table.Column<byte>(type: "tinyint", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: false),
                    LastModifyDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastModifyUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolImages_ApplicationUsers_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SchoolImages_ApplicationUsers_LastModifyUserId",
                        column: x => x.LastModifyUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SchoolImages_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolComments_LastModifyUserId",
                table: "SchoolComments",
                column: "LastModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolComments_Status",
                table: "SchoolComments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolImages_CreationUserId",
                table: "SchoolImages",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolImages_LastModifyUserId",
                table: "SchoolImages",
                column: "LastModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolImages_SchoolId",
                table: "SchoolImages",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolImages_Status",
                table: "SchoolImages",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolComments_ApplicationUsers_LastModifyUserId",
                table: "SchoolComments",
                column: "LastModifyUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolComments_ApplicationUsers_LastModifyUserId",
                table: "SchoolComments");

            migrationBuilder.DropTable(
                name: "SchoolImages");

            migrationBuilder.DropIndex(
                name: "IX_SchoolComments_LastModifyUserId",
                table: "SchoolComments");

            migrationBuilder.DropIndex(
                name: "IX_SchoolComments_Status",
                table: "SchoolComments");

            migrationBuilder.DropColumn(
                name: "LastModifyDate",
                table: "SchoolComments");

            migrationBuilder.DropColumn(
                name: "LastModifyUserId",
                table: "SchoolComments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SchoolComments");
        }
    }
}
