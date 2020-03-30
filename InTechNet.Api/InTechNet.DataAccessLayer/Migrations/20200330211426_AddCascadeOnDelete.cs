using Microsoft.EntityFrameworkCore.Migrations;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class AddCascadeOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendee_hub_HubIdHub",
                schema: "public",
                table: "attendee");

            migrationBuilder.DropForeignKey(
                name: "FK_attendee_pupil_PupilIdPupil",
                schema: "public",
                table: "attendee");

            migrationBuilder.DropForeignKey(
                name: "FK_hub_moderator_ModeratorIdModerator",
                schema: "public",
                table: "hub");

            migrationBuilder.DropForeignKey(
                name: "FK_moderator_subscription_plan_ModeratorSubscriptionPlanIdSubs~",
                schema: "public",
                table: "moderator");

            migrationBuilder.AddForeignKey(
                name: "FK_attendee_hub_HubIdHub",
                schema: "public",
                table: "attendee",
                column: "HubIdHub",
                principalSchema: "public",
                principalTable: "hub",
                principalColumn: "IdHub",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_attendee_pupil_PupilIdPupil",
                schema: "public",
                table: "attendee",
                column: "PupilIdPupil",
                principalSchema: "public",
                principalTable: "pupil",
                principalColumn: "IdPupil",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_hub_moderator_ModeratorIdModerator",
                schema: "public",
                table: "hub",
                column: "ModeratorIdModerator",
                principalSchema: "public",
                principalTable: "moderator",
                principalColumn: "IdModerator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_moderator_subscription_plan_ModeratorSubscriptionPlanIdSubs~",
                schema: "public",
                table: "moderator",
                column: "ModeratorSubscriptionPlanIdSubscriptionPlan",
                principalSchema: "public",
                principalTable: "subscription_plan",
                principalColumn: "IdSubscriptionPlan",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendee_hub_HubIdHub",
                schema: "public",
                table: "attendee");

            migrationBuilder.DropForeignKey(
                name: "FK_attendee_pupil_PupilIdPupil",
                schema: "public",
                table: "attendee");

            migrationBuilder.DropForeignKey(
                name: "FK_hub_moderator_ModeratorIdModerator",
                schema: "public",
                table: "hub");

            migrationBuilder.DropForeignKey(
                name: "FK_moderator_subscription_plan_ModeratorSubscriptionPlanIdSubs~",
                schema: "public",
                table: "moderator");

            migrationBuilder.AddForeignKey(
                name: "FK_attendee_hub_HubIdHub",
                schema: "public",
                table: "attendee",
                column: "HubIdHub",
                principalSchema: "public",
                principalTable: "hub",
                principalColumn: "IdHub",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_attendee_pupil_PupilIdPupil",
                schema: "public",
                table: "attendee",
                column: "PupilIdPupil",
                principalSchema: "public",
                principalTable: "pupil",
                principalColumn: "IdPupil",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_hub_moderator_ModeratorIdModerator",
                schema: "public",
                table: "hub",
                column: "ModeratorIdModerator",
                principalSchema: "public",
                principalTable: "moderator",
                principalColumn: "IdModerator",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_moderator_subscription_plan_ModeratorSubscriptionPlanIdSubs~",
                schema: "public",
                table: "moderator",
                column: "ModeratorSubscriptionPlanIdSubscriptionPlan",
                principalSchema: "public",
                principalTable: "subscription_plan",
                principalColumn: "IdSubscriptionPlan",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
