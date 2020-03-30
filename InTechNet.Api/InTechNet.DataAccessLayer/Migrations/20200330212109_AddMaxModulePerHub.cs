using Microsoft.EntityFrameworkCore.Migrations;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class AddMaxModulePerHub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxModulePerHub",
                schema: "public",
                table: "subscription_plan",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxModulePerHub",
                schema: "public",
                table: "subscription_plan");
        }
    }
}
