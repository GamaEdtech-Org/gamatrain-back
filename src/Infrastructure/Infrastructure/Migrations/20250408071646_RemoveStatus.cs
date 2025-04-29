using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SchoolImages_Status",
                table: "SchoolImages");

            migrationBuilder.DropIndex(
                name: "IX_SchoolComments_Status",
                table: "SchoolComments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SchoolImages");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SchoolComments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "SchoolImages",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "SchoolComments",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolImages_Status",
                table: "SchoolImages",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolComments_Status",
                table: "SchoolComments",
                column: "Status");
        }
    }
}
