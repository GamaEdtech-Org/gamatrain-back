using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.DataSource.Migrations
{
    /// <inheritdoc />
    public partial class LinkSchoolTableToCityAndStateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressCity",
                table: "School");

            migrationBuilder.DropColumn(
                name: "AddressState",
                table: "School");

            migrationBuilder.AddColumn<int>(
                name: "Address_CityId",
                table: "School",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Address_StateId",
                table: "School",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_School_Address_CityId",
                table: "School",
                column: "Address_CityId");

            migrationBuilder.CreateIndex(
                name: "IX_School_Address_StateId",
                table: "School",
                column: "Address_StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_School_City_Address_CityId",
                table: "School",
                column: "Address_CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_School_State_Address_StateId",
                table: "School",
                column: "Address_StateId",
                principalTable: "State",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_School_City_Address_CityId",
                table: "School");

            migrationBuilder.DropForeignKey(
                name: "FK_School_State_Address_StateId",
                table: "School");

            migrationBuilder.DropIndex(
                name: "IX_School_Address_CityId",
                table: "School");

            migrationBuilder.DropIndex(
                name: "IX_School_Address_StateId",
                table: "School");

            migrationBuilder.DropColumn(
                name: "Address_CityId",
                table: "School");

            migrationBuilder.DropColumn(
                name: "Address_StateId",
                table: "School");

            migrationBuilder.AddColumn<string>(
                name: "AddressCity",
                table: "School",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressState",
                table: "School",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
