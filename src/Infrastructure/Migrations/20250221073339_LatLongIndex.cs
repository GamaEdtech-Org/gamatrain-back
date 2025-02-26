using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LatLongIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Schools_Latitude",
                table: "Schools",
                column: "Latitude");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_Longitude",
                table: "Schools",
                column: "Longitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schools_Latitude",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_Longitude",
                table: "Schools");
        }
    }
}
