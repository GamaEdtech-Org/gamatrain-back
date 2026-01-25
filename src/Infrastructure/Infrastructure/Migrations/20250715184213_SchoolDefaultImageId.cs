using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SchoolDefaultImageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DefaultImageId",
                table: "Schools",
                type: "bigint",
                nullable: true);

            migrationBuilder.Sql(@"
UPDATE Schools SET DefaultImageId=(SELECT TOP 1 i.Id FROM SchoolImages i WHERE i.IsDefault = 1 AND i.SchoolId = Id)
");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_DefaultImageId",
                table: "Schools",
                column: "DefaultImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_LastModifyDate_CreationDate",
                table: "Schools",
                columns: new[] { "LastModifyDate", "CreationDate" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_Score",
                table: "Schools",
                column: "Score",
                descending: new bool[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_SchoolImages_DefaultImageId",
                table: "Schools",
                column: "DefaultImageId",
                principalTable: "SchoolImages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schools_SchoolImages_DefaultImageId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_DefaultImageId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_LastModifyDate_CreationDate",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_Score",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "DefaultImageId",
                table: "Schools");
        }
    }
}
