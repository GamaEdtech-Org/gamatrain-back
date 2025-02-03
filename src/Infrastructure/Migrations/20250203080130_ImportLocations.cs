using System.Text;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImportLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_Title_Type_ParentId",
                table: "Locations");

            var bytes = (byte[])Resource1.ResourceManager.GetObject("Locations");
            var sql = UTF8Encoding.UTF8.GetString(bytes);
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Locations_Title_Type_ParentId",
                table: "Locations",
                columns: new[] { "Title", "Type", "ParentId" },
                unique: true,
                filter: "[ParentId] IS NOT NULL");
        }
    }
}
