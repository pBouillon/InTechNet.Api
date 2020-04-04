using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class AddModuleType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleType",
                schema: "public",
                table: "module");

            migrationBuilder.AddColumn<int>(
                name: "IdType",
                schema: "public",
                table: "module",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModuleTypeIdModule",
                schema: "public",
                table: "module",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "module_type",
                schema: "public",
                columns: table => new
                {
                    IdModule = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module_type", x => x.IdModule);
                });

            migrationBuilder.CreateIndex(
                name: "IX_module_ModuleTypeIdModule",
                schema: "public",
                table: "module",
                column: "ModuleTypeIdModule");

            migrationBuilder.AddForeignKey(
                name: "FK_module_module_type_ModuleTypeIdModule",
                schema: "public",
                table: "module",
                column: "ModuleTypeIdModule",
                principalSchema: "public",
                principalTable: "module_type",
                principalColumn: "IdModule",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_module_module_type_ModuleTypeIdModule",
                schema: "public",
                table: "module");

            migrationBuilder.DropTable(
                name: "module_type",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_module_ModuleTypeIdModule",
                schema: "public",
                table: "module");

            migrationBuilder.DropColumn(
                name: "IdType",
                schema: "public",
                table: "module");

            migrationBuilder.DropColumn(
                name: "ModuleTypeIdModule",
                schema: "public",
                table: "module");

            migrationBuilder.AddColumn<string>(
                name: "ModuleType",
                schema: "public",
                table: "module",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);
        }
    }
}
