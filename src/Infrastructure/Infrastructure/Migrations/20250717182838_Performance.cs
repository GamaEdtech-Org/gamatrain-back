using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Performance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Schools_IsDeleted_Name",
                table: "Schools",
                columns: new[] { "IsDeleted", "Name" });

            migrationBuilder.Sql(@"
CREATE INDEX IX_Schools_CountryId_IsDeleted ON [Schools] ([CountryId], [IsDeleted]) 
	INCLUDE ([Name], [StateId], [CreationDate], [LastModifyDate], [CityId], [Email], [PhoneNumber], [WebSite], [Score], [Coordinates], [DefaultImageId])

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schools_IsDeleted_Name",
                table: "Schools");
        }
    }
}
