using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Back.DataSource.Migrations
{
    /// <inheritdoc />
    public partial class RenameAddress_CityIdColumnToAddressCityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_School_City_Address_CityId",
                table: "School");

            migrationBuilder.RenameColumn(
                name: "Address_CityId",
                table: "School",
                newName: "AddressCityId");

            migrationBuilder.RenameIndex(
                name: "IX_School_Address_CityId",
                table: "School",
                newName: "IX_School_AddressCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_School_City_AddressCityId",
                table: "School",
                column: "AddressCityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_School_City_AddressCityId",
                table: "School");

            migrationBuilder.RenameColumn(
                name: "AddressCityId",
                table: "School",
                newName: "Address_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_School_AddressCityId",
                table: "School",
                newName: "IX_School_Address_CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_School_City_Address_CityId",
                table: "School",
                column: "Address_CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
