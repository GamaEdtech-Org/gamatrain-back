using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ApplicationUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ApplicationUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SchoolComments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassesQualityRate = table.Column<byte>(type: "tinyint", nullable: false),
                    EducationRate = table.Column<byte>(type: "tinyint", nullable: false),
                    ITTrainingRate = table.Column<byte>(type: "tinyint", nullable: false),
                    SafetyAndHappinessRate = table.Column<byte>(type: "tinyint", nullable: false),
                    BehaviorRate = table.Column<byte>(type: "tinyint", nullable: false),
                    TuitionRatioRate = table.Column<byte>(type: "tinyint", nullable: false),
                    FacilitiesRate = table.Column<byte>(type: "tinyint", nullable: false),
                    ArtisticActivitiesRate = table.Column<byte>(type: "tinyint", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false),
                    AverageRate = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolComments_ApplicationUsers_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SchoolComments_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Avatar", "FirstName", "LastName" },
                values: new object[] { null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolComments_CreationUserId_SchoolId",
                table: "SchoolComments",
                columns: new[] { "CreationUserId", "SchoolId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolComments_SchoolId",
                table: "SchoolComments",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolComments");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ApplicationUsers");
        }
    }
}
