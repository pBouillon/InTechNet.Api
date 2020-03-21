using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class BillingPlansAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModeratorSubscriptionIdSubscription",
                schema: "public",
                table: "moderator",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HubDescription",
                schema: "public",
                table: "hub",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "subscription",
                schema: "public",
                columns: table => new
                {
                    IdSubscription = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriptionName = table.Column<string>(nullable: true),
                    HubMaxNumber = table.Column<int>(nullable: false),
                    SubscriptionPrice = table.Column<decimal>(nullable: false),
                    AttendeeMaxNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscription", x => x.IdSubscription);
                });

            migrationBuilder.CreateIndex(
                name: "IX_moderator_ModeratorSubscriptionIdSubscription",
                schema: "public",
                table: "moderator",
                column: "ModeratorSubscriptionIdSubscription");

            migrationBuilder.AddForeignKey(
                name: "FK_moderator_subscription_ModeratorSubscriptionIdSubscription",
                schema: "public",
                table: "moderator",
                column: "ModeratorSubscriptionIdSubscription",
                principalSchema: "public",
                principalTable: "subscription",
                principalColumn: "IdSubscription",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_moderator_subscription_ModeratorSubscriptionIdSubscription",
                schema: "public",
                table: "moderator");

            migrationBuilder.DropTable(
                name: "subscription",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_moderator_ModeratorSubscriptionIdSubscription",
                schema: "public",
                table: "moderator");

            migrationBuilder.DropColumn(
                name: "ModeratorSubscriptionIdSubscription",
                schema: "public",
                table: "moderator");

            migrationBuilder.DropColumn(
                name: "HubDescription",
                schema: "public",
                table: "hub");
        }
    }
}
