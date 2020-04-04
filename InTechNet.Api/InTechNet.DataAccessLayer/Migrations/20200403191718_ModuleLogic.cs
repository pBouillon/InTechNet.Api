using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class ModuleLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "module",
                schema: "public",
                columns: table => new
                {
                    IdModule = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleType = table.Column<string>(maxLength: 64, nullable: true),
                    ModuleName = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module", x => x.IdModule);
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
                name: "CurrentModules",
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
                    table.PrimaryKey("PK_CurrentModules", x => x.IdCurrentModule);
                    table.ForeignKey(
                        name: "FK_CurrentModules_attendee_AttendeeIdAttendee",
                        column: x => x.AttendeeIdAttendee,
                        principalSchema: "public",
                        principalTable: "attendee",
                        principalColumn: "IdAttendee",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrentModules_module_ModuleIdModule",
                        column: x => x.ModuleIdModule,
                        principalSchema: "public",
                        principalTable: "module",
                        principalColumn: "IdModule",
                        onDelete: ReferentialAction.Restrict);
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
                    IdNextResource = table.Column<int>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "selected_module",
                schema: "public",
                columns: table => new
                {
                    IdSelectedModule = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdHub = table.Column<int>(nullable: false),
                    HubIdHub = table.Column<int>(nullable: true),
                    IdModule = table.Column<int>(nullable: false),
                    ModuleIdModule = table.Column<int>(nullable: true),
                    IsSelected = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_selected_module", x => x.IdSelectedModule);
                    table.ForeignKey(
                        name: "FK_selected_module_hub_HubIdHub",
                        column: x => x.HubIdHub,
                        principalSchema: "public",
                        principalTable: "hub",
                        principalColumn: "IdHub",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_selected_module_module_ModuleIdModule",
                        column: x => x.ModuleIdModule,
                        principalSchema: "public",
                        principalTable: "module",
                        principalColumn: "IdModule",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_CurrentModules_AttendeeIdAttendee",
                table: "CurrentModules",
                column: "AttendeeIdAttendee");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentModules_ModuleIdModule",
                table: "CurrentModules",
                column: "ModuleIdModule");

            migrationBuilder.CreateIndex(
                name: "IX_resource_ModuleIdModule",
                schema: "public",
                table: "resource",
                column: "ModuleIdModule");

            migrationBuilder.CreateIndex(
                name: "IX_selected_module_HubIdHub",
                schema: "public",
                table: "selected_module",
                column: "HubIdHub");

            migrationBuilder.CreateIndex(
                name: "IX_selected_module_ModuleIdModule",
                schema: "public",
                table: "selected_module",
                column: "ModuleIdModule");

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
                name: "CurrentModules");

            migrationBuilder.DropTable(
                name: "selected_module",
                schema: "public");

            migrationBuilder.DropTable(
                name: "state",
                schema: "public");

            migrationBuilder.DropTable(
                name: "topic",
                schema: "public");

            migrationBuilder.DropTable(
                name: "resource",
                schema: "public");

            migrationBuilder.DropTable(
                name: "tag",
                schema: "public");

            migrationBuilder.DropTable(
                name: "module",
                schema: "public");
        }
    }
}
