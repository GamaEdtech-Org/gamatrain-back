using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnstoApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "ApplicationUsers",
                type: "varchar",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "ApplicationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Section",
                table: "ApplicationUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "ApplicationUsers");
        }
    }
}
