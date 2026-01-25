using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExamSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TestSubmissions",
                table: "TestSubmissions");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "TestSubmissions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestSubmissions",
                table: "TestSubmissions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ExamSubmissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Valid = table.Column<int>(type: "int", nullable: false),
                    Invalid = table.Column<int>(type: "int", nullable: false),
                    NoAnswer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamSubmissions_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubmissions_UserId_ExamId",
                table: "ExamSubmissions",
                columns: new[] { "UserId", "ExamId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamSubmissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestSubmissions",
                table: "TestSubmissions");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TestSubmissions",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestSubmissions",
                table: "TestSubmissions",
                column: "Id");
        }
    }
}
