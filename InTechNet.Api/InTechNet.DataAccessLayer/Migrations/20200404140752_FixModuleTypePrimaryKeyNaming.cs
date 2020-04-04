using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class FixModuleTypePrimaryKeyNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_module_module_type_ModuleTypeIdModule",
                schema: "public",
                table: "module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_module_type",
                schema: "public",
                table: "module_type");

            migrationBuilder.DropIndex(
                name: "IX_module_ModuleTypeIdModule",
                schema: "public",
                table: "module");

            migrationBuilder.DropColumn(
                name: "IdModule",
                schema: "public",
                table: "module_type");

            migrationBuilder.DropColumn(
                name: "ModuleTypeIdModule",
                schema: "public",
                table: "module");

            migrationBuilder.AddColumn<int>(
                name: "IdModuleType",
                schema: "public",
                table: "module_type",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ModuleTypeIdModuleType",
                schema: "public",
                table: "module",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_module_type",
                schema: "public",
                table: "module_type",
                column: "IdModuleType");

            migrationBuilder.CreateIndex(
                name: "IX_module_ModuleTypeIdModuleType",
                schema: "public",
                table: "module",
                column: "ModuleTypeIdModuleType");

            migrationBuilder.AddForeignKey(
                name: "FK_module_module_type_ModuleTypeIdModuleType",
                schema: "public",
                table: "module",
                column: "ModuleTypeIdModuleType",
                principalSchema: "public",
                principalTable: "module_type",
                principalColumn: "IdModuleType",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_module_module_type_ModuleTypeIdModuleType",
                schema: "public",
                table: "module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_module_type",
                schema: "public",
                table: "module_type");

            migrationBuilder.DropIndex(
                name: "IX_module_ModuleTypeIdModuleType",
                schema: "public",
                table: "module");

            migrationBuilder.DropColumn(
                name: "IdModuleType",
                schema: "public",
                table: "module_type");

            migrationBuilder.DropColumn(
                name: "ModuleTypeIdModuleType",
                schema: "public",
                table: "module");

            migrationBuilder.AddColumn<int>(
                name: "IdModule",
                schema: "public",
                table: "module_type",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ModuleTypeIdModule",
                schema: "public",
                table: "module",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_module_type",
                schema: "public",
                table: "module_type",
                column: "IdModule");

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
    }
}
