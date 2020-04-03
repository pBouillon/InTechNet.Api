using Microsoft.EntityFrameworkCore.Migrations;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class FixForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdNextResource",
                schema: "public",
                table: "resource",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "fk_next_resource",
                schema: "public",
                table: "resource",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_resource_fk_next_resource",
                schema: "public",
                table: "resource",
                column: "fk_next_resource");

            migrationBuilder.AddForeignKey(
                name: "FK_resource_resource_fk_next_resource",
                schema: "public",
                table: "resource",
                column: "fk_next_resource",
                principalSchema: "public",
                principalTable: "resource",
                principalColumn: "IdResource",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_resource_resource_fk_next_resource",
                schema: "public",
                table: "resource");

            migrationBuilder.DropIndex(
                name: "IX_resource_fk_next_resource",
                schema: "public",
                table: "resource");

            migrationBuilder.DropColumn(
                name: "fk_next_resource",
                schema: "public",
                table: "resource");

            migrationBuilder.AlterColumn<int>(
                name: "IdNextResource",
                schema: "public",
                table: "resource",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
