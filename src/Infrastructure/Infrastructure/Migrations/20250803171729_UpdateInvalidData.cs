using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvalidData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                table: "ApplicationUsers");
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "ApplicationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ApplicationUsers");
            migrationBuilder.AddColumn<long>(
                name: "SchoolId",
                table: "ApplicationUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.Sql("UPDATE ApplicationUsers SET CityId=NULL WHERE CityId=0");
            migrationBuilder.Sql("UPDATE ApplicationUsers SET SchoolId=NULL WHERE SchoolId=0");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_CityId",
                table: "ApplicationUsers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_SchoolId",
                table: "ApplicationUsers",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Locations_CityId",
                table: "ApplicationUsers",
                column: "CityId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Schools_SchoolId",
                table: "ApplicationUsers",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Locations_CityId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Schools_SchoolId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_CityId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_SchoolId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ApplicationUsers");
        }
    }
}
