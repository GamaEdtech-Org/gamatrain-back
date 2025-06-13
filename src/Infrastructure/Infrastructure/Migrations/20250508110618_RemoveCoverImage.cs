using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCoverImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageId",
                table: "Schools");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "SchoolImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "SchoolImages");

            migrationBuilder.AddColumn<string>(
                name: "CoverImageId",
                table: "Schools",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
