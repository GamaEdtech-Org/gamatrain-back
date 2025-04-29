using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IdentifierId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SchoolTags_SchoolId",
                table: "SchoolTags");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_PostId",
                table: "PostTags");

            migrationBuilder.AddColumn<long>(
                name: "ContributionId",
                table: "SchoolImages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTags_SchoolId_TagId",
                table: "SchoolTags",
                columns: new[] { "SchoolId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolImages_ContributionId",
                table: "SchoolImages",
                column: "ContributionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_PostId_TagId",
                table: "PostTags",
                columns: new[] { "PostId", "TagId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolImages_Contributions_ContributionId",
                table: "SchoolImages",
                column: "ContributionId",
                principalTable: "Contributions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolImages_Contributions_ContributionId",
                table: "SchoolImages");

            migrationBuilder.DropIndex(
                name: "IX_SchoolTags_SchoolId_TagId",
                table: "SchoolTags");

            migrationBuilder.DropIndex(
                name: "IX_SchoolImages_ContributionId",
                table: "SchoolImages");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_PostId_TagId",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "ContributionId",
                table: "SchoolImages");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTags_SchoolId",
                table: "SchoolTags",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_PostId",
                table: "PostTags",
                column: "PostId");
        }
    }
}
