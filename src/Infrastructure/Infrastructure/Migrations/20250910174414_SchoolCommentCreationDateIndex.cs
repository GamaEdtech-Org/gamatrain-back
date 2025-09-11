using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SchoolCommentCreationDateIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SchoolComments_SchoolId",
                table: "SchoolComments");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolComments_SchoolId_CreationDate",
                table: "SchoolComments",
                columns: new[] { "SchoolId", "CreationDate" });

            migrationBuilder.AddColumn<int>(
                name: "CurrentBalance",
                table: "ApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
UPDATE u
SET u.CurrentBalance = d.CurrentBalance
FROM ApplicationUsers u
INNER JOIN (
	SELECT UserId,CurrentBalance FROM Transactions WHERE Id IN (SELECT MAX(t.Id) FROM Transactions t GROUP BY t.UserId)
) d ON u.id = d.UserId
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SchoolComments_SchoolId_CreationDate",
                table: "SchoolComments");

            migrationBuilder.DropColumn(
                name: "CurrentBalance",
                table: "ApplicationUsers");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolComments_SchoolId",
                table: "SchoolComments",
                column: "SchoolId");
        }
    }
}
