using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class CreateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "pupil",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PupilNickname = table.Column<string>(maxLength: 64, nullable: true),
                    PupilEmail = table.Column<string>(maxLength: 128, nullable: true),
                    PupilPassword = table.Column<string>(nullable: true),
                    PupilSalt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pupil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subscription_plan",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriptionPlanName = table.Column<string>(nullable: true),
                    MaxHubPerModeratorAccount = table.Column<int>(nullable: false),
                    MaxModulePerHub = table.Column<int>(nullable: false),
                    SubscriptionPlanPrice = table.Column<decimal>(nullable: false),
                    MaxAttendeesPerHub = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscription_plan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "moderator",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModeratorNickname = table.Column<string>(maxLength: 64, nullable: true),
                    ModeratorEmail = table.Column<string>(maxLength: 128, nullable: true),
                    ModeratorPassword = table.Column<string>(nullable: true),
                    ModeratorSalt = table.Column<string>(nullable: true),
                    ModeratorSubscriptionPlanId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moderator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_moderator_subscription_plan_ModeratorSubscriptionPlanId",
                        column: x => x.ModeratorSubscriptionPlanId,
                        principalSchema: "public",
                        principalTable: "subscription_plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "module",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleDescription = table.Column<string>(maxLength: 128, nullable: true),
                    SubscriptionPlanId = table.Column<int>(nullable: true),
                    ModuleName = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_module_subscription_plan_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalSchema: "public",
                        principalTable: "subscription_plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "hub",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HubName = table.Column<string>(nullable: true),
                    HubLink = table.Column<string>(nullable: true),
                    HubDescription = table.Column<string>(nullable: true),
                    HubCreationDate = table.Column<DateTime>(nullable: false),
                    ModeratorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hub", x => x.Id);
                    table.ForeignKey(
                        name: "FK_hub_moderator_ModeratorId",
                        column: x => x.ModeratorId,
                        principalSchema: "public",
                        principalTable: "moderator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resource",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    NextResourceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_resource_module_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "public",
                        principalTable: "module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resource_resource_NextResourceId",
                        column: x => x.NextResourceId,
                        principalSchema: "public",
                        principalTable: "resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "topic",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<int>(nullable: true),
                    TagId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_topic_module_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "public",
                        principalTable: "module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_topic_tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "public",
                        principalTable: "tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attendee",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PupilId = table.Column<int>(nullable: true),
                    HubId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attendee_hub_HubId",
                        column: x => x.HubId,
                        principalSchema: "public",
                        principalTable: "hub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attendee_pupil_PupilId",
                        column: x => x.PupilId,
                        principalSchema: "public",
                        principalTable: "pupil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "available_module",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HubId = table.Column<int>(nullable: true),
                    ModuleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_available_module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_available_module_hub_HubId",
                        column: x => x.HubId,
                        principalSchema: "public",
                        principalTable: "hub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_available_module_module_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "public",
                        principalTable: "module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "current_module",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<int>(nullable: true),
                    AttendeeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_current_module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_current_module_attendee_AttendeeId",
                        column: x => x.AttendeeId,
                        principalSchema: "public",
                        principalTable: "attendee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_current_module_module_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "public",
                        principalTable: "module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "state",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResourceId = table.Column<int>(nullable: true),
                    AttendeeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_state", x => x.Id);
                    table.ForeignKey(
                        name: "FK_state_attendee_AttendeeId",
                        column: x => x.AttendeeId,
                        principalSchema: "public",
                        principalTable: "attendee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_state_resource_ResourceId",
                        column: x => x.ResourceId,
                        principalSchema: "public",
                        principalTable: "resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "subscription_plan",
                columns: new[] { "Id", "MaxAttendeesPerHub", "MaxHubPerModeratorAccount", "MaxModulePerHub", "SubscriptionPlanName", "SubscriptionPlanPrice" },
                values: new object[,]
                {
                    { 1, 32, 3, 3, "Standard", 0.0m },
                    { 2, 50, 5, 5, "Premium", 5.0m },
                    { 3, 60, 10, 15, "Platinum", 10.0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_attendee_HubId",
                schema: "public",
                table: "attendee",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_attendee_PupilId",
                schema: "public",
                table: "attendee",
                column: "PupilId");

            migrationBuilder.CreateIndex(
                name: "IX_available_module_HubId",
                schema: "public",
                table: "available_module",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_available_module_ModuleId",
                schema: "public",
                table: "available_module",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_current_module_AttendeeId",
                schema: "public",
                table: "current_module",
                column: "AttendeeId");

            migrationBuilder.CreateIndex(
                name: "IX_current_module_ModuleId",
                schema: "public",
                table: "current_module",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "index_hub_link",
                schema: "public",
                table: "hub",
                column: "HubLink");

            migrationBuilder.CreateIndex(
                name: "IX_hub_ModeratorId",
                schema: "public",
                table: "hub",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "index_moderator_email",
                schema: "public",
                table: "moderator",
                column: "ModeratorEmail");

            migrationBuilder.CreateIndex(
                name: "index_moderator_nickname",
                schema: "public",
                table: "moderator",
                column: "ModeratorNickname");

            migrationBuilder.CreateIndex(
                name: "IX_moderator_ModeratorSubscriptionPlanId",
                schema: "public",
                table: "moderator",
                column: "ModeratorSubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_module_SubscriptionPlanId",
                schema: "public",
                table: "module",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "index_pupil_email",
                schema: "public",
                table: "pupil",
                column: "PupilEmail");

            migrationBuilder.CreateIndex(
                name: "index_pupil_nickname",
                schema: "public",
                table: "pupil",
                column: "PupilNickname");

            migrationBuilder.CreateIndex(
                name: "IX_resource_ModuleId",
                schema: "public",
                table: "resource",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_resource_NextResourceId",
                schema: "public",
                table: "resource",
                column: "NextResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_state_AttendeeId",
                schema: "public",
                table: "state",
                column: "AttendeeId");

            migrationBuilder.CreateIndex(
                name: "IX_state_ResourceId",
                schema: "public",
                table: "state",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "index_tag_name",
                schema: "public",
                table: "tag",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_topic_ModuleId",
                schema: "public",
                table: "topic",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_topic_TagId",
                schema: "public",
                table: "topic",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "available_module",
                schema: "public");

            migrationBuilder.DropTable(
                name: "current_module",
                schema: "public");

            migrationBuilder.DropTable(
                name: "state",
                schema: "public");

            migrationBuilder.DropTable(
                name: "topic",
                schema: "public");

            migrationBuilder.DropTable(
                name: "attendee",
                schema: "public");

            migrationBuilder.DropTable(
                name: "resource",
                schema: "public");

            migrationBuilder.DropTable(
                name: "tag",
                schema: "public");

            migrationBuilder.DropTable(
                name: "hub",
                schema: "public");

            migrationBuilder.DropTable(
                name: "pupil",
                schema: "public");

            migrationBuilder.DropTable(
                name: "module",
                schema: "public");

            migrationBuilder.DropTable(
                name: "moderator",
                schema: "public");

            migrationBuilder.DropTable(
                name: "subscription_plan",
                schema: "public");
        }
    }
}
