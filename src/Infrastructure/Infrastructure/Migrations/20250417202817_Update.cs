using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_ApplicationUsers_CreationUserId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_ApplicationUsers_CreationUserId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolTags_ApplicationUsers_CreationUserId",
                table: "SchoolTags");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_ApplicationUsers_CreationUserId",
                table: "PostTags",
                column: "CreationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_ApplicationUsers_CreationUserId",
                table: "Reactions",
                column: "CreationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolTags_ApplicationUsers_CreationUserId",
                table: "SchoolTags",
                column: "CreationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_ApplicationUsers_CreationUserId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_ApplicationUsers_CreationUserId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolTags_ApplicationUsers_CreationUserId",
                table: "SchoolTags");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_ApplicationUsers_CreationUserId",
                table: "PostTags",
                column: "CreationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_ApplicationUsers_CreationUserId",
                table: "Reactions",
                column: "CreationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolTags_ApplicationUsers_CreationUserId",
                table: "SchoolTags",
                column: "CreationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
