using Microsoft.EntityFrameworkCore.Migrations;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class AddSubscriptionPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "subscription_plan",
                keyColumn: "IdSubscriptionPlan",
                keyValue: 1,
                column: "MaxModulePerHub",
                value: 3);

            migrationBuilder.InsertData(
                schema: "public",
                table: "subscription_plan",
                columns: new[] { "IdSubscriptionPlan", "MaxAttendeesPerHub", "MaxHubPerModeratorAccount", "MaxModulePerHub", "SubscriptionPlanName", "SubscriptionPlanPrice" },
                values: new object[,]
                {
                    { 3, 60, 10, 15, "Platinium", 10.0m },
                    { 2, 50, 5, 5, "Premium", 5.0m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "subscription_plan",
                keyColumn: "IdSubscriptionPlan",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "subscription_plan",
                keyColumn: "IdSubscriptionPlan",
                keyValue: 3);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "subscription_plan",
                keyColumn: "IdSubscriptionPlan",
                keyValue: 1,
                column: "MaxModulePerHub",
                value: 0);
        }
    }
}
