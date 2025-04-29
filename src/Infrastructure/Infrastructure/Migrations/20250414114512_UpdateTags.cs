using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamaEdtech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TagType = table.Column<byte>(type: "tinyint", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: false),
                    LastModifyDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastModifyUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_ApplicationUsers_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tags_ApplicationUsers_LastModifyUserId",
                        column: x => x.LastModifyUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CreationUserId",
                table: "Tags",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_LastModifyUserId",
                table: "Tags",
                column: "LastModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagType",
                table: "Tags",
                column: "TagType");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagType_Name",
                table: "Tags",
                columns: new[] { "TagType", "Name" },
                unique: true);

            migrationBuilder.AddColumn<long>(
                name: "TagId",
                table: "SchoolImages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SchoolTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolTags_ApplicationUsers_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTags_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolImages_TagId",
                table: "SchoolImages",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTags_CreationUserId",
                table: "SchoolTags",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTags_SchoolId",
                table: "SchoolTags",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTags_TagId",
                table: "SchoolTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolImages_Tags_TagId",
                table: "SchoolImages",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolImages_Tags_TagId",
                table: "SchoolImages");

            migrationBuilder.DropTable(
                name: "SchoolTags");

            migrationBuilder.DropIndex(
                name: "IX_SchoolImages_TagId",
                table: "SchoolImages");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "SchoolImages");
        }
    }
}
