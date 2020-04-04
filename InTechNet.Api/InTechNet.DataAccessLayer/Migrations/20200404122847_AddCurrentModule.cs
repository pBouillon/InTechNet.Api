using Microsoft.EntityFrameworkCore.Migrations;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class AddCurrentModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentModules_attendee_AttendeeIdAttendee",
                table: "CurrentModules");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentModules_module_ModuleIdModule",
                table: "CurrentModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentModules",
                table: "CurrentModules");

            migrationBuilder.RenameTable(
                name: "CurrentModules",
                newName: "current_module",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentModules_ModuleIdModule",
                schema: "public",
                table: "current_module",
                newName: "IX_current_module_ModuleIdModule");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentModules_AttendeeIdAttendee",
                schema: "public",
                table: "current_module",
                newName: "IX_current_module_AttendeeIdAttendee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_current_module",
                schema: "public",
                table: "current_module",
                column: "IdCurrentModule");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "subscription_plan",
                keyColumn: "IdSubscriptionPlan",
                keyValue: 3,
                column: "SubscriptionPlanName",
                value: "Platinum");

            migrationBuilder.AddForeignKey(
                name: "FK_current_module_attendee_AttendeeIdAttendee",
                schema: "public",
                table: "current_module",
                column: "AttendeeIdAttendee",
                principalSchema: "public",
                principalTable: "attendee",
                principalColumn: "IdAttendee",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_current_module_module_ModuleIdModule",
                schema: "public",
                table: "current_module",
                column: "ModuleIdModule",
                principalSchema: "public",
                principalTable: "module",
                principalColumn: "IdModule",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_current_module_attendee_AttendeeIdAttendee",
                schema: "public",
                table: "current_module");

            migrationBuilder.DropForeignKey(
                name: "FK_current_module_module_ModuleIdModule",
                schema: "public",
                table: "current_module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_current_module",
                schema: "public",
                table: "current_module");

            migrationBuilder.RenameTable(
                name: "current_module",
                schema: "public",
                newName: "CurrentModules");

            migrationBuilder.RenameIndex(
                name: "IX_current_module_ModuleIdModule",
                table: "CurrentModules",
                newName: "IX_CurrentModules_ModuleIdModule");

            migrationBuilder.RenameIndex(
                name: "IX_current_module_AttendeeIdAttendee",
                table: "CurrentModules",
                newName: "IX_CurrentModules_AttendeeIdAttendee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentModules",
                table: "CurrentModules",
                column: "IdCurrentModule");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "subscription_plan",
                keyColumn: "IdSubscriptionPlan",
                keyValue: 3,
                column: "SubscriptionPlanName",
                value: "Platinium");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentModules_attendee_AttendeeIdAttendee",
                table: "CurrentModules",
                column: "AttendeeIdAttendee",
                principalSchema: "public",
                principalTable: "attendee",
                principalColumn: "IdAttendee",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentModules_module_ModuleIdModule",
                table: "CurrentModules",
                column: "ModuleIdModule",
                principalSchema: "public",
                principalTable: "module",
                principalColumn: "IdModule",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
