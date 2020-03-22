using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class AddBillingPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModeratorSubscriptionPlanIdSubscriptionPlan",
                schema: "public",
                table: "moderator",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HubDescription",
                schema: "public",
                table: "hub",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "subscription_plan",
                schema: "public",
                columns: table => new
                {
                    IdSubscriptionPlan = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriptionPlanName = table.Column<string>(nullable: true),
                    MaxHubPerModeratorAccount = table.Column<int>(nullable: false),
                    SubscriptionPlanPrice = table.Column<decimal>(nullable: false),
                    MaxAttendeesPerHub = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscription_plan", x => x.IdSubscriptionPlan);
                });

            migrationBuilder.CreateIndex(
                name: "IX_moderator_ModeratorSubscriptionPlanIdSubscriptionPlan",
                schema: "public",
                table: "moderator",
                column: "ModeratorSubscriptionPlanIdSubscriptionPlan");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_moderator_subscription_plan_ModeratorSubscriptionPlanIdSubs~",
                schema: "public",
                table: "moderator");

            migrationBuilder.DropTable(
                name: "subscription_plan",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_moderator_ModeratorSubscriptionPlanIdSubscriptionPlan",
                schema: "public",
                table: "moderator");

            migrationBuilder.DropColumn(
                name: "ModeratorSubscriptionPlanIdSubscriptionPlan",
                schema: "public",
                table: "moderator");

            migrationBuilder.DropColumn(
                name: "HubDescription",
                schema: "public",
                table: "hub");
        }
    }
}
