using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InTechNet.DataAccessLayer.Migrations
{
    public partial class CleanDataIntablesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "hub",
                keyColumn: "IdHub",
                keyValue: 1,
                columns: new[] { "HubCreationDate", "HubLink", "HubName" },
                values: new object[] { new DateTime(2020, 3, 15, 1, 37, 41, 171, DateTimeKind.Local).AddTicks(5448), "hub-link", "hub-name" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "moderator",
                keyColumn: "IdModerator",
                keyValue: 1,
                columns: new[] { "ModeratorEmail", "ModeratorNickname", "ModeratorPassword", "ModeratorSalt" },
                values: new object[] { "moderator@intechnet.io", "moderator", "720E39C10B81B3652B149FA74B3757AD1453F10FD4445F2A1AB4196BF2D23CE5D64A8DCD6DE157194853F35CC160F8A851155261B82B271BB81AD0B700AF9992", "moderator-salt" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "pupil",
                keyColumn: "IdPupil",
                keyValue: 1,
                columns: new[] { "PupilEmail", "PupilNickname", "PupilPassword", "PupilSalt" },
                values: new object[] { "pupil@intechnet.io", "pupil", "CF28AF1039C0348CE7715232444454F47E085D6859913BFE531008D1BEF4992D27D7A3301E9CC70004F0F42513676FC01B941C848160351D389BBC3A264DC0E2", "pupil-salt" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "hub",
                keyColumn: "IdHub",
                keyValue: 1,
                columns: new[] { "HubCreationDate", "HubLink", "HubName" },
                values: new object[] { new DateTime(2020, 3, 14, 16, 27, 9, 357, DateTimeKind.Local).AddTicks(3960), "hublink1", "supername" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "moderator",
                keyColumn: "IdModerator",
                keyValue: 1,
                columns: new[] { "ModeratorEmail", "ModeratorNickname", "ModeratorPassword", "ModeratorSalt" },
                values: new object[] { "test@test.com", "modeNick", "CC3827BF052E6B257CE6FBE896077A132448552CA6746CD538A11039950636ABD7440927318E5D9EBBD151C6A93364B8F5AD761A871403227395F4D99D01E34A", "lesaltcestbien" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "pupil",
                keyColumn: "IdPupil",
                keyValue: 1,
                columns: new[] { "PupilEmail", "PupilNickname", "PupilPassword", "PupilSalt" },
                values: new object[] { "pupil@pupil.com", "pupilNick", "4230B63D16DCEF8861AA9BE6F93B46F2E2ED20EC6C3E7E6001CDEC44DE1186BA015D98F19D3D5C43D38F84CBD00FDC977058066791A2AF7ACFE8863F92C71F8B", "leselcestdrole" });
        }
    }
}
