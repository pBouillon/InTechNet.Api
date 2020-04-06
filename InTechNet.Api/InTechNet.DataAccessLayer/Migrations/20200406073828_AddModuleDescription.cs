using Microsoft.EntityFrameworkCore.Migrations;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class AddModuleDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "public",
                table: "module");

            migrationBuilder.AddColumn<string>(
                name: "ModuleDescription",
                schema: "public",
                table: "module",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModuleName",
                schema: "public",
                table: "module",
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleDescription",
                schema: "public",
                table: "module");

            migrationBuilder.DropColumn(
                name: "ModuleName",
                schema: "public",
                table: "module");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "public",
                table: "module",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);
        }
    }
}
