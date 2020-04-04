using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class ReworkAvailableModuleLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "module_type",
                schema: "public",
                columns: table => new
                {
                    IdModuleType = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module_type", x => x.IdModuleType);
                });

            migrationBuilder.CreateTable(
                name: "pupil",
                schema: "public",
                columns: table => new
                {
                    IdPupil = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PupilNickname = table.Column<string>(maxLength: 64, nullable: true),
                    PupilEmail = table.Column<string>(maxLength: 128, nullable: true),
                    PupilPassword = table.Column<string>(nullable: true),
                    PupilSalt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pupil", x => x.IdPupil);
                });

            migrationBuilder.CreateTable(
                name: "subscription_plan",
                schema: "public",
                columns: table => new
                {
                    IdSubscriptionPlan = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriptionPlanName = table.Column<string>(nullable: true),
                    MaxHubPerModeratorAccount = table.Column<int>(nullable: false),
                    MaxModulePerHub = table.Column<int>(nullable: false),
                    SubscriptionPlanPrice = table.Column<decimal>(nullable: false),
                    MaxAttendeesPerHub = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscription_plan", x => x.IdSubscriptionPlan);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                schema: "public",
                columns: table => new
                {
                    IdTag = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.IdTag);
                });

            migrationBuilder.CreateTable(
                name: "module",
                schema: "public",
                columns: table => new
                {
                    IdModule = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdType = table.Column<int>(nullable: false),
                    ModuleTypeIdModuleType = table.Column<int>(nullable: true),
                    ModuleName = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module", x => x.IdModule);
                    table.ForeignKey(
                        name: "FK_module_module_type_ModuleTypeIdModuleType",
                        column: x => x.ModuleTypeIdModuleType,
                        principalSchema: "public",
                        principalTable: "module_type",
                        principalColumn: "IdModuleType",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "moderator",
                schema: "public",
                columns: table => new
                {
                    IdModerator = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModeratorNickname = table.Column<string>(maxLength: 64, nullable: true),
                    ModeratorEmail = table.Column<string>(maxLength: 128, nullable: true),
                    ModeratorPassword = table.Column<string>(nullable: true),
                    ModeratorSalt = table.Column<string>(nullable: true),
                    ModeratorSubscriptionPlanIdSubscriptionPlan = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moderator", x => x.IdModerator);
                    table.ForeignKey(
                        name: "FK_moderator_subscription_plan_ModeratorSubscriptionPlanIdSubs~",
                        column: x => x.ModeratorSubscriptionPlanIdSubscriptionPlan,
                        principalSchema: "public",
                        principalTable: "subscription_plan",
                        principalColumn: "IdSubscriptionPlan",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "resource",
                schema: "public",
                columns: table => new
                {
                    IdResource = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdModule = table.Column<int>(nullable: false),
                    ModuleIdModule = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IdNextResource = table.Column<int>(nullable: true),
                    fk_next_resource = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resource", x => x.IdResource);
                    table.ForeignKey(
                        name: "FK_resource_module_ModuleIdModule",
                        column: x => x.ModuleIdModule,
                        principalSchema: "public",
                        principalTable: "module",
                        principalColumn: "IdModule",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resource_resource_fk_next_resource",
                        column: x => x.fk_next_resource,
                        principalSchema: "public",
                        principalTable: "resource",
                        principalColumn: "IdResource",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "topic",
                schema: "public",
                columns: table => new
                {
                    IdTopic = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdModule = table.Column<int>(nullable: false),
                    ModuleIdModule = table.Column<int>(nullable: true),
                    IdTag = table.Column<int>(nullable: false),
                    TagIdTag = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic", x => x.IdTopic);
                    table.ForeignKey(
                        name: "FK_topic_module_ModuleIdModule",
                        column: x => x.ModuleIdModule,
                        principalSchema: "public",
                        principalTable: "module",
                        principalColumn: "IdModule",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_topic_tag_TagIdTag",
                        column: x => x.TagIdTag,
                        principalSchema: "public",
                        principalTable: "tag",
                        principalColumn: "IdTag",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hub",
                schema: "public",
                columns: table => new
                {
                    IdHub = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HubName = table.Column<string>(nullable: true),
                    HubLink = table.Column<string>(nullable: true),
                    HubDescription = table.Column<string>(nullable: true),
                    HubCreationDate = table.Column<DateTime>(nullable: false),
                    ModeratorIdModerator = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hub", x => x.IdHub);
                    table.ForeignKey(
                        name: "FK_hub_moderator_ModeratorIdModerator",
                        column: x => x.ModeratorIdModerator,
                        principalSchema: "public",
                        principalTable: "moderator",
                        principalColumn: "IdModerator",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attendee",
                schema: "public",
                columns: table => new
                {
                    IdAttendee = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPupil = table.Column<int>(nullable: false),
                    PupilIdPupil = table.Column<int>(nullable: true),
                    IdHub = table.Column<int>(nullable: false),
                    HubIdHub = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendee", x => x.IdAttendee);
                    table.ForeignKey(
                        name: "FK_attendee_hub_HubIdHub",
                        column: x => x.HubIdHub,
                        principalSchema: "public",
                        principalTable: "hub",
                        principalColumn: "IdHub",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attendee_pupil_PupilIdPupil",
                        column: x => x.PupilIdPupil,
                        principalSchema: "public",
                        principalTable: "pupil",
                        principalColumn: "IdPupil",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "available_module",
                schema: "public",
                columns: table => new
                {
                    IdAvailableModule = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdHub = table.Column<int>(nullable: false),
                    HubIdHub = table.Column<int>(nullable: true),
                    IdModule = table.Column<int>(nullable: false),
                    ModuleIdModule = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_available_module", x => x.IdAvailableModule);
                    table.ForeignKey(
                        name: "FK_available_module_hub_HubIdHub",
                        column: x => x.HubIdHub,
                        principalSchema: "public",
                        principalTable: "hub",
                        principalColumn: "IdHub",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_available_module_module_ModuleIdModule",
                        column: x => x.ModuleIdModule,
                        principalSchema: "public",
                        principalTable: "module",
                        principalColumn: "IdModule",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "current_module",
                schema: "public",
                columns: table => new
                {
                    IdCurrentModule = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdModule = table.Column<int>(nullable: false),
                    ModuleIdModule = table.Column<int>(nullable: true),
                    IdAttendee = table.Column<int>(nullable: false),
                    AttendeeIdAttendee = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_current_module", x => x.IdCurrentModule);
                    table.ForeignKey(
                        name: "FK_current_module_attendee_AttendeeIdAttendee",
                        column: x => x.AttendeeIdAttendee,
                        principalSchema: "public",
                        principalTable: "attendee",
                        principalColumn: "IdAttendee",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_current_module_module_ModuleIdModule",
                        column: x => x.ModuleIdModule,
                        principalSchema: "public",
                        principalTable: "module",
                        principalColumn: "IdModule",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "state",
                schema: "public",
                columns: table => new
                {
                    IdState = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdResource = table.Column<int>(nullable: false),
                    ResourceIdResource = table.Column<int>(nullable: true),
                    IdAttendee = table.Column<int>(nullable: false),
                    AttendeeIdAttendee = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_state", x => x.IdState);
                    table.ForeignKey(
                        name: "FK_state_attendee_AttendeeIdAttendee",
                        column: x => x.AttendeeIdAttendee,
                        principalSchema: "public",
                        principalTable: "attendee",
                        principalColumn: "IdAttendee",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_state_resource_ResourceIdResource",
                        column: x => x.ResourceIdResource,
                        principalSchema: "public",
                        principalTable: "resource",
                        principalColumn: "IdResource",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "subscription_plan",
                columns: new[] { "IdSubscriptionPlan", "MaxAttendeesPerHub", "MaxHubPerModeratorAccount", "MaxModulePerHub", "SubscriptionPlanName", "SubscriptionPlanPrice" },
                values: new object[,]
                {
                    { 1, 32, 3, 3, "Standard", 0.0m },
                    { 2, 50, 5, 5, "Premium", 5.0m },
                    { 3, 60, 10, 15, "Platinum", 10.0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_attendee_HubIdHub",
                schema: "public",
                table: "attendee",
                column: "HubIdHub");

            migrationBuilder.CreateIndex(
                name: "IX_attendee_PupilIdPupil",
                schema: "public",
                table: "attendee",
                column: "PupilIdPupil");

            migrationBuilder.CreateIndex(
                name: "IX_available_module_HubIdHub",
                schema: "public",
                table: "available_module",
                column: "HubIdHub");

            migrationBuilder.CreateIndex(
                name: "IX_available_module_ModuleIdModule",
                schema: "public",
                table: "available_module",
                column: "ModuleIdModule");

            migrationBuilder.CreateIndex(
                name: "IX_current_module_AttendeeIdAttendee",
                schema: "public",
                table: "current_module",
                column: "AttendeeIdAttendee");

            migrationBuilder.CreateIndex(
                name: "IX_current_module_ModuleIdModule",
                schema: "public",
                table: "current_module",
                column: "ModuleIdModule");

            migrationBuilder.CreateIndex(
                name: "index_hub_link",
                schema: "public",
                table: "hub",
                column: "HubLink");

            migrationBuilder.CreateIndex(
                name: "IX_hub_ModeratorIdModerator",
                schema: "public",
                table: "hub",
                column: "ModeratorIdModerator");

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
                name: "IX_moderator_ModeratorSubscriptionPlanIdSubscriptionPlan",
                schema: "public",
                table: "moderator",
                column: "ModeratorSubscriptionPlanIdSubscriptionPlan");

            migrationBuilder.CreateIndex(
                name: "IX_module_ModuleTypeIdModuleType",
                schema: "public",
                table: "module",
                column: "ModuleTypeIdModuleType");

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
                name: "IX_resource_ModuleIdModule",
                schema: "public",
                table: "resource",
                column: "ModuleIdModule");

            migrationBuilder.CreateIndex(
                name: "IX_resource_fk_next_resource",
                schema: "public",
                table: "resource",
                column: "fk_next_resource");

            migrationBuilder.CreateIndex(
                name: "IX_state_AttendeeIdAttendee",
                schema: "public",
                table: "state",
                column: "AttendeeIdAttendee");

            migrationBuilder.CreateIndex(
                name: "IX_state_ResourceIdResource",
                schema: "public",
                table: "state",
                column: "ResourceIdResource");

            migrationBuilder.CreateIndex(
                name: "index_tag_name",
                schema: "public",
                table: "tag",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_topic_ModuleIdModule",
                schema: "public",
                table: "topic",
                column: "ModuleIdModule");

            migrationBuilder.CreateIndex(
                name: "IX_topic_TagIdTag",
                schema: "public",
                table: "topic",
                column: "TagIdTag");
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
                name: "module_type",
                schema: "public");

            migrationBuilder.DropTable(
                name: "subscription_plan",
                schema: "public");
        }
    }
}
