using Microsoft.EntityFrameworkCore.Migrations;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class AddTagNameIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "index_tag_name",
                schema: "public",
                table: "tag",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "index_tag_name",
                schema: "public",
                table: "tag");
        }
    }
}
