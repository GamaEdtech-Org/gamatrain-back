using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SchoolId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SchoolImages_SchoolId",
                table: "SchoolImages");
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolImages_Schools_SchoolId",
                table: "SchoolImages");

            migrationBuilder.DropIndex(
                name: "IX_SchoolComments_SchoolId",
                table: "SchoolComments");
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolComments_Schools_SchoolId",
                table: "SchoolComments");

            migrationBuilder.Sql("DROP INDEX IF EXISTS [SPATIAL_Schools] ON [dbo].[Schools]");
            migrationBuilder.DropPrimaryKey(
                name: "PK_Schools",
                table: "Schools");
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Schools",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
            migrationBuilder.AddPrimaryKey(
                name: "PK_Schools",
                table: "Schools",
                column: "Id");
            migrationBuilder.Sql(@"
CREATE SPATIAL INDEX [SPATIAL_Schools] ON [dbo].[Schools]
(
    [Coordinates]
)USING  GEOGRAPHY_GRID
WITH (GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM),
CELLS_PER_OBJECT = 16, PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");

            migrationBuilder.AlterColumn<long>(
                name: "SchoolId",
                table: "SchoolImages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
            migrationBuilder.CreateIndex(
                name: "IX_SchoolImages_SchoolId",
                table: "SchoolImages",
                column: "SchoolId");
            migrationBuilder.AddForeignKey(
                name: "FK_SchoolImages_Schools_SchoolId",
                table: "SchoolImages",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");

            migrationBuilder.AlterColumn<long>(
                name: "SchoolId",
                table: "SchoolComments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
            migrationBuilder.CreateIndex(
                name: "IX_SchoolComments_SchoolId",
                table: "SchoolComments",
                column: "SchoolId");
            migrationBuilder.AddForeignKey(
                name: "FK_SchoolComments_Schools_SchoolId",
                table: "SchoolComments",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Schools",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "SchoolId",
                table: "SchoolImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "SchoolId",
                table: "SchoolComments",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
