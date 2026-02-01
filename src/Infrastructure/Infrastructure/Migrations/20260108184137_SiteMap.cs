using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SiteMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentifierId = table.Column<long>(type: "bigint", nullable: false),
                    ItemType = table.Column<byte>(type: "tinyint", nullable: false),
                    Priority = table.Column<double>(type: "float", nullable: true),
                    ChangeFrequency = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteMaps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteMaps_IdentifierId_ItemType",
                table: "SiteMaps",
                columns: new[] { "ItemType", "IdentifierId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteMaps");
        }
    }
}
