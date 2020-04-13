using Microsoft.EntityFrameworkCore.Migrations;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class AddCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_current_module_attendee_AttendeeId",
                schema: "public",
                table: "current_module");

            migrationBuilder.DropForeignKey(
                name: "FK_current_module_module_ModuleId",
                schema: "public",
                table: "current_module");

            migrationBuilder.AddForeignKey(
                name: "FK_current_module_attendee_AttendeeId",
                schema: "public",
                table: "current_module",
                column: "AttendeeId",
                principalSchema: "public",
                principalTable: "attendee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_current_module_module_ModuleId",
                schema: "public",
                table: "current_module",
                column: "ModuleId",
                principalSchema: "public",
                principalTable: "module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_current_module_attendee_AttendeeId",
                schema: "public",
                table: "current_module");

            migrationBuilder.DropForeignKey(
                name: "FK_current_module_module_ModuleId",
                schema: "public",
                table: "current_module");

            migrationBuilder.AddForeignKey(
                name: "FK_current_module_attendee_AttendeeId",
                schema: "public",
                table: "current_module",
                column: "AttendeeId",
                principalSchema: "public",
                principalTable: "attendee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_current_module_module_ModuleId",
                schema: "public",
                table: "current_module",
                column: "ModuleId",
                principalSchema: "public",
                principalTable: "module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
