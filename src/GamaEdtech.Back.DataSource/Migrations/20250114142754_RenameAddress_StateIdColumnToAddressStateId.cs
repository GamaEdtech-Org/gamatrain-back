using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Back.DataSource.Migrations
{
    /// <inheritdoc />
    public partial class RenameAddress_StateIdColumnToAddressStateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_School_State_Address_StateId",
                table: "School");

            migrationBuilder.RenameColumn(
                name: "Address_StateId",
                table: "School",
                newName: "AddressStateId");

            migrationBuilder.RenameIndex(
                name: "IX_School_Address_StateId",
                table: "School",
                newName: "IX_School_AddressStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_School_State_AddressStateId",
                table: "School",
                column: "AddressStateId",
                principalTable: "State",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_School_State_AddressStateId",
                table: "School");

            migrationBuilder.RenameColumn(
                name: "AddressStateId",
                table: "School",
                newName: "Address_StateId");

            migrationBuilder.RenameIndex(
                name: "IX_School_AddressStateId",
                table: "School",
                newName: "IX_School_Address_StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_School_State_Address_StateId",
                table: "School",
                column: "Address_StateId",
                principalTable: "State",
                principalColumn: "Id");
        }
    }
}
