using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class InitialDataSchemeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "moderator",
                schema: "public",
                columns: table => new
                {
                    IdModerator = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModeratorNickname = table.Column<string>(maxLength: 64, nullable: true),
                    ModeratorEmail = table.Column<string>(maxLength: 128, nullable: true),
                    ModeratorPassword = table.Column<string>(maxLength: 64, nullable: true),
                    ModeratorSalt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moderator", x => x.IdModerator);
                });

            migrationBuilder.CreateTable(
                name: "pupil",
                schema: "public",
                columns: table => new
                {
                    IdPupil = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PupilNickname = table.Column<string>(nullable: true),
                    PupilEmail = table.Column<string>(nullable: true),
                    PupilPassword = table.Column<string>(nullable: true),
                    PupilSalt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pupil", x => x.IdPupil);
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_attendee_pupil_PupilIdPupil",
                        column: x => x.PupilIdPupil,
                        principalSchema: "public",
                        principalTable: "pupil",
                        principalColumn: "IdPupil",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_hub_HubName",
                schema: "public",
                table: "hub",
                column: "HubName");

            migrationBuilder.CreateIndex(
                name: "IX_hub_ModeratorIdModerator",
                schema: "public",
                table: "hub",
                column: "ModeratorIdModerator");

            migrationBuilder.CreateIndex(
                name: "IX_moderator_ModeratorNickname",
                schema: "public",
                table: "moderator",
                column: "ModeratorNickname");

            migrationBuilder.CreateIndex(
                name: "IX_pupil_PupilNickname",
                schema: "public",
                table: "pupil",
                column: "PupilNickname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attendee",
                schema: "public");

            migrationBuilder.DropTable(
                name: "hub",
                schema: "public");

            migrationBuilder.DropTable(
                name: "pupil",
                schema: "public");

            migrationBuilder.DropTable(
                name: "moderator",
                schema: "public");
        }
    }
}
