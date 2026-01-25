using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentifierId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE Contributions SET IdentifierId=(SELECT Id FROM Posts WHERE Slug IS NOT NULL AND Slug=JSON_VALUE([Data], '$.Slug'))
WHERE IdentifierId IS NULL AND CategoryType=4
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //empty
        }
    }
}
