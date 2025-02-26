using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SchoolLocation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schools_Latitude",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_Longitude",
                table: "Schools");

            migrationBuilder.Sql("CREATE SPATIAL INDEX SPATIAL_Schools_Location ON dbo.Schools(Location) USING GEOMETRY_GRID    WITH(BOUNDING_BOX = (xmin = 0.0, ymin = 0.0, xmax = 500, ymax = 200), GRIDS = (LEVEL_1 = MEDIUM, LEVEL_2 = MEDIUM, LEVEL_3 = MEDIUM, LEVEL_4 = MEDIUM), CELLS_PER_OBJECT = 16, STATISTICS_NORECOMPUTE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
