using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecruitPro.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAndPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "action", "created_at", "created_by", "deleted_at", "deleted_by", "description", "is_active", "is_deleted", "modified_at", "modified_by", "name", "permission_ext_id", "resource", "row_version" },
                values: new object[,]
                {
                    { new Guid("01a7eb08-420e-0140-bd31-f7926fc2d7f8"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Skill.Create", new Guid("c8830e45-1ecc-043f-d25b-56ca7a941c2d"), null, new byte[] { 8, 235, 167, 1, 14, 66, 64, 1, 189, 49, 247, 146, 111, 194, 215, 248 } },
                    { new Guid("04fe65fa-64e8-d009-5882-21d829f5816a"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Resume.Download", new Guid("72145839-eb50-f510-c88d-a65700fbbbe1"), null, new byte[] { 250, 101, 254, 4, 232, 100, 9, 208, 88, 130, 33, 216, 41, 245, 129, 106 } },
                    { new Guid("05261bba-9548-e41f-d1c2-38f94bb71955"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Job.Create", new Guid("160a5d9b-07a6-9fe1-d82f-dfe2755e481b"), null, new byte[] { 186, 27, 38, 5, 72, 149, 31, 228, 209, 194, 56, 249, 75, 183, 25, 85 } },
                    { new Guid("12672cb1-68bc-49ce-4ee7-f8155452e26e"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Application.Create", new Guid("705c4fc4-a0b5-4ae5-4df8-f6193087ae00"), null, new byte[] { 177, 44, 103, 18, 188, 104, 206, 73, 78, 231, 248, 21, 84, 82, 226, 110 } },
                    { new Guid("15fe83a8-6390-df6e-0c78-e1c6b3cc183e"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Reporting.Dashboard.View", new Guid("f25638f4-7ca2-bf6c-0563-2c14c244c017"), null, new byte[] { 168, 131, 254, 21, 144, 99, 110, 223, 12, 120, 225, 198, 179, 204, 24, 62 } },
                    { new Guid("1765dab6-4360-2b46-b10b-ca6e696fe69f"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Identity.User.View", new Guid("2a3b6ff3-041d-1e7c-970f-1c2cad6ea3d9"), null, new byte[] { 182, 218, 101, 23, 96, 67, 70, 43, 177, 11, 202, 110, 105, 111, 230, 159 } },
                    { new Guid("2083e2bb-6cb5-9a66-7eed-95f199125fec"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Notifications.Template.Manage", new Guid("4aa4ff9f-6bbc-fa1b-d84a-307bc368d987"), null, new byte[] { 187, 226, 131, 32, 181, 108, 102, 154, 126, 237, 149, 241, 153, 18, 95, 236 } },
                    { new Guid("2144c9b0-2f6f-077b-320e-300e9934019e"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Interview.Schedule", new Guid("75f0818c-605d-541d-593f-72c086bd383d"), null, new byte[] { 176, 201, 68, 33, 111, 47, 123, 7, 50, 14, 48, 14, 153, 52, 1, 158 } },
                    { new Guid("2884bbe2-51aa-5948-6df0-95f47b949e78"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Identity.Role.View", new Guid("c5a2f534-b480-3c24-6728-85ef079fb7d2"), null, new byte[] { 226, 187, 132, 40, 170, 81, 72, 89, 109, 240, 149, 244, 123, 148, 158, 120 } },
                    { new Guid("31791b9f-c984-1bb4-cbf0-44fcc65bf777"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Candidate.Create", new Guid("b8e33ed5-1870-47e0-466b-6fa58cb6acad"), null, new byte[] { 159, 27, 121, 49, 132, 201, 180, 27, 203, 240, 68, 252, 198, 91, 247, 119 } },
                    { new Guid("40c239d6-9d6c-903c-9f98-286a13c1110a"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Identity.Role.Manage", new Guid("a9a77c7a-1684-dfa9-13b7-f70cc12b2ffc"), null, new byte[] { 214, 57, 194, 64, 108, 157, 60, 144, 159, 152, 40, 106, 19, 193, 17, 10 } },
                    { new Guid("4fb980f9-59be-aee3-707b-d9d271d683f5"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Job.Publish", new Guid("20518b52-83e6-7691-1337-7f8a00de7cc2"), null, new byte[] { 249, 128, 185, 79, 190, 89, 227, 174, 112, 123, 217, 210, 113, 214, 131, 245 } },
                    { new Guid("65dbc826-f4a9-8554-e537-0f4d53d8d1f2"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Candidate.View", new Guid("7f8ad1e3-83e1-8475-a80e-1a9104af5802"), null, new byte[] { 38, 200, 219, 101, 169, 244, 84, 133, 229, 55, 15, 77, 83, 216, 209, 242 } },
                    { new Guid("680be577-aed0-23bf-c109-0770aa8b4dff"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Skill.View", new Guid("1cacab76-4404-265d-34bd-bae836292db4"), null, new byte[] { 119, 229, 11, 104, 208, 174, 191, 35, 193, 9, 7, 112, 170, 139, 77, 255 } },
                    { new Guid("757dcc4f-707d-5b9b-3713-59228b0ac543"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Recruiter.View", new Guid("1ab437bb-4cb3-1a82-9bd0-cee17346879f"), null, new byte[] { 79, 204, 125, 117, 125, 112, 155, 91, 55, 19, 89, 34, 139, 10, 197, 67 } },
                    { new Guid("934ad611-6513-73e9-9310-37b6e2326177"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Client.Create", new Guid("38cba7c5-b71c-0f7a-b09c-790afac5fe28"), null, new byte[] { 17, 214, 74, 147, 19, 101, 233, 115, 147, 16, 55, 182, 226, 50, 97, 119 } },
                    { new Guid("a8e8f15d-dd68-a40f-d1f8-66412f5a49cc"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Notifications.Template.View", new Guid("58e2d3ef-09fb-b7b7-80a6-2f5784ade866"), null, new byte[] { 93, 241, 232, 168, 104, 221, 15, 164, 209, 248, 102, 65, 47, 90, 73, 204 } },
                    { new Guid("ade50811-4596-96e4-913d-37f34aacd9e6"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Department.Create", new Guid("fe2b6ee8-41b2-a556-2a62-406c8b651c25"), null, new byte[] { 17, 8, 229, 173, 150, 69, 228, 150, 145, 61, 55, 243, 74, 172, 217, 230 } },
                    { new Guid("b5d0f234-0528-f82c-660e-23997e30a690"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Department.View", new Guid("e687b4a2-087f-dc72-4bc9-c7acab09ec0c"), null, new byte[] { 52, 242, 208, 181, 40, 5, 44, 248, 102, 14, 35, 153, 126, 48, 166, 144 } },
                    { new Guid("b9b47947-f770-0de9-8be5-ba8b67e42263"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Client.View", new Guid("b28cc50c-6cf8-75af-50a8-6b3eae53b3d3"), null, new byte[] { 71, 121, 180, 185, 112, 247, 233, 13, 139, 229, 186, 139, 103, 228, 34, 99 } },
                    { new Guid("bc9de91a-026b-febf-57ff-0f580eb84789"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Recruiter.Create", new Guid("cacdb3bc-07ba-a048-05f3-09cdbe575ef4"), null, new byte[] { 26, 233, 157, 188, 107, 2, 191, 254, 87, 255, 15, 88, 14, 184, 71, 137 } },
                    { new Guid("bcb6bb17-1916-ddb8-397a-125067962bb2"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Offer.View", new Guid("eecc5806-45ed-6a57-1a70-0936cff595d5"), null, new byte[] { 23, 187, 182, 188, 22, 25, 184, 221, 57, 122, 18, 80, 103, 150, 43, 178 } },
                    { new Guid("bde05a2b-a0b5-47e8-2b75-9f4b348d69b3"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Job.View", new Guid("1884d2ae-0981-16a2-ca32-d610a415b3a0"), null, new byte[] { 43, 90, 224, 189, 181, 160, 232, 71, 43, 117, 159, 75, 52, 141, 105, 179 } },
                    { new Guid("c69add39-a786-ff31-c160-7fe64e6ca493"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Interview.RecordFeedback", new Guid("bb628239-3f0c-830c-7672-5211fbc44979"), null, new byte[] { 57, 221, 154, 198, 134, 167, 49, 255, 193, 96, 127, 230, 78, 108, 164, 147 } },
                    { new Guid("cd59d989-9d8f-ab98-fbc6-a06cd3d795a8"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Application.View", new Guid("5caa5a88-e1cf-52cd-1cab-81aa1d955b4a"), null, new byte[] { 137, 217, 89, 205, 143, 157, 152, 171, 251, 198, 160, 108, 211, 215, 149, 168 } },
                    { new Guid("cd5ea0fc-cbb0-dcc1-0099-57d7af4aa7c3"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Notifications.Log.View", new Guid("3b58979a-a0e5-6f95-8c23-d31f4234c4f9"), null, new byte[] { 252, 160, 94, 205, 176, 203, 193, 220, 0, 153, 87, 215, 175, 74, 167, 195 } },
                    { new Guid("d011c6b4-f6c0-9416-7ab5-e87242322274"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Interview.View", new Guid("aae8fa87-9dce-8251-28f7-d36ee8c64b85"), null, new byte[] { 180, 198, 17, 208, 192, 246, 22, 148, 122, 181, 232, 114, 66, 50, 34, 116 } },
                    { new Guid("d5254b68-9969-1be5-3fee-6a30bc62acda"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Identity.User.Manage", new Guid("58d8c3bd-5718-9e42-b046-e9bdc353a67b"), null, new byte[] { 104, 75, 37, 213, 105, 153, 229, 27, 63, 238, 106, 48, 188, 98, 172, 218 } },
                    { new Guid("d724bd4e-7463-7936-69cf-59d0d5fbdac1"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Resume.Upload", new Guid("e955aae4-6011-c530-59d3-558c4160f316"), null, new byte[] { 78, 189, 36, 215, 99, 116, 54, 121, 105, 207, 89, 208, 213, 251, 218, 193 } },
                    { new Guid("ddc182f7-6225-814c-33ce-9c938b90004d"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Offer.Manage", new Guid("442d7355-005e-c40b-576a-dd60c95ec1ca"), null, new byte[] { 247, 130, 193, 221, 37, 98, 76, 129, 51, 206, 156, 147, 139, 144, 0, 77 } },
                    { new Guid("e03c8020-f40e-e4f9-b718-204b733a4698"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Identity.Permission.View", new Guid("1431256d-6aa6-5c1e-6ea4-199993b53da8"), null, new byte[] { 32, 128, 60, 224, 14, 244, 249, 228, 183, 24, 32, 75, 115, 58, 70, 152 } },
                    { new Guid("e070e7e3-cee6-0b66-816d-57728e7b2b60"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Identity.AuditLog.View", new Guid("7ea7a7ef-26ee-b339-4dfa-7684fcbdd44f"), null, new byte[] { 227, 231, 112, 224, 230, 206, 102, 11, 129, 109, 87, 114, 142, 123, 43, 96 } },
                    { new Guid("e3014711-6f98-7517-e57a-4921932d0d88"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Offer.Create", new Guid("85c7d9e3-ada7-e090-a17e-dddeeebe6d2a"), null, new byte[] { 17, 71, 1, 227, 152, 111, 23, 117, 229, 122, 73, 33, 147, 45, 13, 136 } },
                    { new Guid("f598f262-214c-bf89-fc6a-3ab58d716af5"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Application.MoveStage", new Guid("6b734257-934f-8c60-d625-465f78db95d3"), null, new byte[] { 98, 242, 152, 245, 76, 33, 137, 191, 252, 106, 58, 181, 141, 113, 106, 245 } },
                    { new Guid("feb91607-53a4-6736-3e7c-369172310890"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Identity.Permission.Manage", new Guid("67f7a75f-d649-f89b-8b3b-a573956718b9"), null, new byte[] { 7, 22, 185, 254, 164, 83, 54, 103, 62, 124, 54, 145, 114, 49, 8, 144 } }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "code", "created_at", "created_by", "deleted_at", "deleted_by", "description", "is_active", "is_deleted", "is_system", "modified_at", "modified_by", "name", "role_ext_id", "row_version" },
                values: new object[,]
                {
                    { new Guid("4eaf3e0e-0f8e-e7a1-33fb-ebb8807730b8"), "ACCOUNT_MANAGER", new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Primary liaison between client and delivery organization.", true, false, false, null, null, "Account Manager", new Guid("bf6e03a5-6cef-04ab-7b76-0c51ec4a9baa"), new byte[] { 14, 62, 175, 78, 142, 15, 161, 231, 51, 251, 235, 184, 128, 119, 48, 184 } },
                    { new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"), "TEAM_LEADER", new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Leads recruiters and ensures timely delivery against assigned requisitions.", true, false, false, null, null, "Team Leader", new Guid("0b0fc593-aba9-74c4-edeb-efce2a76188c"), new byte[] { 22, 47, 216, 143, 32, 98, 122, 176, 55, 103, 24, 109, 129, 2, 214, 175 } },
                    { new Guid("d649856a-053e-1937-04ea-e899c6041d40"), "PROFILE_UPLOADER", new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Prepares complete candidate records for recruiters.", true, false, false, null, null, "Profile Uploader", new Guid("10f8421b-1120-5697-ad82-8ece89561ba8"), new byte[] { 106, 133, 73, 214, 62, 5, 55, 25, 4, 234, 232, 153, 198, 4, 29, 64 } },
                    { new Guid("dbe3bcb2-d68f-c6a7-ae43-5f6acc06c724"), "DELIVERY_MANAGER", new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Owns delivery execution, resource planning, SLA compliance, and operational performance.", true, false, false, null, null, "Delivery Manager", new Guid("03977d83-5a3a-1cce-9451-a5e4b2eb2f6c"), new byte[] { 178, 188, 227, 219, 143, 214, 167, 198, 174, 67, 95, 106, 204, 6, 199, 36 } },
                    { new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), "ADMINISTRATOR", new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "Owns platform governance, security, configuration, and operational administration.", true, false, true, null, null, "Administrator", new Guid("e08dcd16-1540-5b5d-873a-677d3f4c0e84"), new byte[] { 99, 151, 171, 224, 231, 184, 252, 219, 244, 122, 12, 8, 36, 57, 11, 253 } }
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "id", "created_at", "created_by", "deleted_at", "deleted_by", "is_deleted", "modified_at", "modified_by", "permission_id", "role_id", "row_version" },
                values: new object[,]
                {
                    { new Guid("00a9f22d-def1-4678-7b6b-0e608db9fe0a"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("2144c9b0-2f6f-077b-320e-300e9934019e"), new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"), new byte[] { 45, 242, 169, 0, 241, 222, 120, 70, 123, 107, 14, 96, 141, 185, 254, 10 } },
                    { new Guid("030b9d75-16ae-6575-5c30-33b08ba7523b"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("d011c6b4-f6c0-9416-7ab5-e87242322274"), new Guid("4eaf3e0e-0f8e-e7a1-33fb-ebb8807730b8"), new byte[] { 117, 157, 11, 3, 174, 22, 117, 101, 92, 48, 51, 176, 139, 167, 82, 59 } },
                    { new Guid("073a4315-1e14-e1a9-692f-c7ede437a6d9"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("b9b47947-f770-0de9-8be5-ba8b67e42263"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 21, 67, 58, 7, 20, 30, 169, 225, 105, 47, 199, 237, 228, 55, 166, 217 } },
                    { new Guid("0762c3b1-94f8-18c1-bc6d-d9a63fd87f66"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("65dbc826-f4a9-8554-e537-0f4d53d8d1f2"), new Guid("d649856a-053e-1937-04ea-e899c6041d40"), new byte[] { 177, 195, 98, 7, 248, 148, 193, 24, 188, 109, 217, 166, 63, 216, 127, 102 } },
                    { new Guid("085a54ef-7422-f088-4de5-1259692b6987"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("c69add39-a786-ff31-c160-7fe64e6ca493"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 239, 84, 90, 8, 34, 116, 136, 240, 77, 229, 18, 89, 105, 43, 105, 135 } },
                    { new Guid("0aaaacba-ef1c-0ee2-dda2-af44126902a5"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("2144c9b0-2f6f-077b-320e-300e9934019e"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 186, 172, 170, 10, 28, 239, 226, 14, 221, 162, 175, 68, 18, 105, 2, 165 } },
                    { new Guid("0bf75260-a7db-7b40-258e-7523b6a9e801"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("d011c6b4-f6c0-9416-7ab5-e87242322274"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 96, 82, 247, 11, 219, 167, 64, 123, 37, 142, 117, 35, 182, 169, 232, 1 } },
                    { new Guid("0d190790-8659-69ae-2f7a-299ef7133d20"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("b5d0f234-0528-f82c-660e-23997e30a690"), new Guid("dbe3bcb2-d68f-c6a7-ae43-5f6acc06c724"), new byte[] { 144, 7, 25, 13, 89, 134, 174, 105, 47, 122, 41, 158, 247, 19, 61, 32 } },
                    { new Guid("10b37995-d61f-06a1-8a65-6cd3b49be0ed"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("31791b9f-c984-1bb4-cbf0-44fcc65bf777"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 149, 121, 179, 16, 31, 214, 161, 6, 138, 101, 108, 211, 180, 155, 224, 237 } },
                    { new Guid("123b3178-4b4e-1975-08b1-9c11b4068924"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("bde05a2b-a0b5-47e8-2b75-9f4b348d69b3"), new Guid("dbe3bcb2-d68f-c6a7-ae43-5f6acc06c724"), new byte[] { 120, 49, 59, 18, 78, 75, 117, 25, 8, 177, 156, 17, 180, 6, 137, 36 } },
                    { new Guid("12ba1716-0eb2-52a4-adcb-bd0f6a5f9aae"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("b9b47947-f770-0de9-8be5-ba8b67e42263"), new Guid("4eaf3e0e-0f8e-e7a1-33fb-ebb8807730b8"), new byte[] { 22, 23, 186, 18, 178, 14, 164, 82, 173, 203, 189, 15, 106, 95, 154, 174 } },
                    { new Guid("1567aa41-8db0-4fe0-36cf-0c800275b8ae"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("4fb980f9-59be-aee3-707b-d9d271d683f5"), new Guid("4eaf3e0e-0f8e-e7a1-33fb-ebb8807730b8"), new byte[] { 65, 170, 103, 21, 176, 141, 224, 79, 54, 207, 12, 128, 2, 117, 184, 174 } },
                    { new Guid("18a68d38-0782-208c-f5d7-8ba082c2504c"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("65dbc826-f4a9-8554-e537-0f4d53d8d1f2"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 56, 141, 166, 24, 130, 7, 140, 32, 245, 215, 139, 160, 130, 194, 80, 76 } },
                    { new Guid("19bc0c4d-bf89-522a-ab35-f23eed07959f"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("bc9de91a-026b-febf-57ff-0f580eb84789"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 77, 12, 188, 25, 137, 191, 42, 82, 171, 53, 242, 62, 237, 7, 149, 159 } },
                    { new Guid("1e077679-4755-d044-2538-aaf29da0b707"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("1765dab6-4360-2b46-b10b-ca6e696fe69f"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 121, 118, 7, 30, 85, 71, 68, 208, 37, 56, 170, 242, 157, 160, 183, 7 } },
                    { new Guid("1e8cd71f-2161-8756-213c-11f800fd91eb"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("f598f262-214c-bf89-fc6a-3ab58d716af5"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 31, 215, 140, 30, 97, 33, 86, 135, 33, 60, 17, 248, 0, 253, 145, 235 } },
                    { new Guid("1f057c3e-d0c9-e27a-7176-fc4a826c7afe"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("f598f262-214c-bf89-fc6a-3ab58d716af5"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 62, 124, 5, 31, 201, 208, 122, 226, 113, 118, 252, 74, 130, 108, 122, 254 } },
                    { new Guid("2519c4d9-cbd2-4478-bfc6-7e3e426e81f8"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("cd59d989-9d8f-ab98-fbc6-a06cd3d795a8"), new Guid("dbe3bcb2-d68f-c6a7-ae43-5f6acc06c724"), new byte[] { 217, 196, 25, 37, 210, 203, 120, 68, 191, 198, 126, 62, 66, 110, 129, 248 } },
                    { new Guid("27df36f7-2b6e-bb94-b25a-ad4ff2dcd8e6"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("757dcc4f-707d-5b9b-3713-59228b0ac543"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 247, 54, 223, 39, 110, 43, 148, 187, 178, 90, 173, 79, 242, 220, 216, 230 } },
                    { new Guid("2b42fb3a-6553-1e63-0f38-9a4fa575dd70"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("d724bd4e-7463-7936-69cf-59d0d5fbdac1"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 58, 251, 66, 43, 83, 101, 99, 30, 15, 56, 154, 79, 165, 117, 221, 112 } },
                    { new Guid("2b5b6ca3-9bf5-5aac-ab8f-722255dc0261"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("d011c6b4-f6c0-9416-7ab5-e87242322274"), new Guid("dbe3bcb2-d68f-c6a7-ae43-5f6acc06c724"), new byte[] { 163, 108, 91, 43, 245, 155, 172, 90, 171, 143, 114, 34, 85, 220, 2, 97 } },
                    { new Guid("2c84f84e-b33c-0fcc-6c3c-3aa1ccf731ee"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("bde05a2b-a0b5-47e8-2b75-9f4b348d69b3"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 78, 248, 132, 44, 60, 179, 204, 15, 108, 60, 58, 161, 204, 247, 49, 238 } },
                    { new Guid("2e584648-5c63-b91c-048b-bb93d6ffa552"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("d011c6b4-f6c0-9416-7ab5-e87242322274"), new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"), new byte[] { 72, 70, 88, 46, 99, 92, 28, 185, 4, 139, 187, 147, 214, 255, 165, 82 } },
                    { new Guid("3078d6ac-2d2b-cb11-1cf8-596332069fc7"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("a8e8f15d-dd68-a40f-d1f8-66412f5a49cc"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 172, 214, 120, 48, 43, 45, 17, 203, 28, 248, 89, 99, 50, 6, 159, 199 } },
                    { new Guid("34f2c8ec-724b-7e78-23f4-e627d0a6be84"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("cd59d989-9d8f-ab98-fbc6-a06cd3d795a8"), new Guid("4eaf3e0e-0f8e-e7a1-33fb-ebb8807730b8"), new byte[] { 236, 200, 242, 52, 75, 114, 120, 126, 35, 244, 230, 39, 208, 166, 190, 132 } },
                    { new Guid("37230f94-5cbb-79b1-0eb1-d7d447b99ffb"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("4fb980f9-59be-aee3-707b-d9d271d683f5"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 148, 15, 35, 55, 187, 92, 177, 121, 14, 177, 215, 212, 71, 185, 159, 251 } },
                    { new Guid("3b9436bc-5c42-35fb-472f-44fe95a715c5"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("65dbc826-f4a9-8554-e537-0f4d53d8d1f2"), new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"), new byte[] { 188, 54, 148, 59, 66, 92, 251, 53, 71, 47, 68, 254, 149, 167, 21, 197 } },
                    { new Guid("3be35dd2-d9b7-3a7d-2ec2-9b05d8916a11"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("bcb6bb17-1916-ddb8-397a-125067962bb2"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 210, 93, 227, 59, 183, 217, 125, 58, 46, 194, 155, 5, 216, 145, 106, 17 } },
                    { new Guid("3f6b711c-7611-8fda-4d07-712d1b6d17ce"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("934ad611-6513-73e9-9310-37b6e2326177"), new Guid("4eaf3e0e-0f8e-e7a1-33fb-ebb8807730b8"), new byte[] { 28, 113, 107, 63, 17, 118, 218, 143, 77, 7, 113, 45, 27, 109, 23, 206 } },
                    { new Guid("4ef56edf-20c3-bec8-3171-14acbe517da8"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("c69add39-a786-ff31-c160-7fe64e6ca493"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 223, 110, 245, 78, 195, 32, 200, 190, 49, 113, 20, 172, 190, 81, 125, 168 } },
                    { new Guid("50e243dd-c1e3-f5f3-06c8-c751ee515207"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("05261bba-9548-e41f-d1c2-38f94bb71955"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 221, 67, 226, 80, 227, 193, 243, 245, 6, 200, 199, 81, 238, 81, 82, 7 } },
                    { new Guid("5268a9ef-0762-7f4c-5d49-e8860be1c733"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("bde05a2b-a0b5-47e8-2b75-9f4b348d69b3"), new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"), new byte[] { 239, 169, 104, 82, 98, 7, 76, 127, 93, 73, 232, 134, 11, 225, 199, 51 } },
                    { new Guid("56095114-0eb2-f76b-6703-75e4f2e1659d"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("934ad611-6513-73e9-9310-37b6e2326177"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 20, 81, 9, 86, 178, 14, 107, 247, 103, 3, 117, 228, 242, 225, 101, 157 } },
                    { new Guid("57c57ec4-6610-536e-c0a7-6c8029f02f72"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("f598f262-214c-bf89-fc6a-3ab58d716af5"), new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"), new byte[] { 196, 126, 197, 87, 16, 102, 110, 83, 192, 167, 108, 128, 41, 240, 47, 114 } },
                    { new Guid("59771783-b148-b636-d09a-010e9d323764"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("2083e2bb-6cb5-9a66-7eed-95f199125fec"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 131, 23, 119, 89, 72, 177, 54, 182, 208, 154, 1, 14, 157, 50, 55, 100 } },
                    { new Guid("5ce0c1c4-2cf3-0d3e-c53b-850396cb5815"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("bcb6bb17-1916-ddb8-397a-125067962bb2"), new Guid("dbe3bcb2-d68f-c6a7-ae43-5f6acc06c724"), new byte[] { 196, 193, 224, 92, 243, 44, 62, 13, 197, 59, 133, 3, 150, 203, 88, 21 } },
                    { new Guid("63da56e2-3904-f2aa-04f5-5b18d66f73e0"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("04fe65fa-64e8-d009-5882-21d829f5816a"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 226, 86, 218, 99, 4, 57, 170, 242, 4, 245, 91, 24, 214, 111, 115, 224 } },
                    { new Guid("65f9c188-29bd-ae91-3ed1-1ae10080d650"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("cd59d989-9d8f-ab98-fbc6-a06cd3d795a8"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 136, 193, 249, 101, 189, 41, 145, 174, 62, 209, 26, 225, 0, 128, 214, 80 } },
                    { new Guid("6bb19e2c-1b01-6e58-e9ad-6286dacc70ff"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("12672cb1-68bc-49ce-4ee7-f8155452e26e"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 44, 158, 177, 107, 1, 27, 88, 110, 233, 173, 98, 134, 218, 204, 112, 255 } },
                    { new Guid("723e80dd-a3a7-995d-5b10-f5c8f8b457c6"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("c69add39-a786-ff31-c160-7fe64e6ca493"), new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"), new byte[] { 221, 128, 62, 114, 167, 163, 93, 153, 91, 16, 245, 200, 248, 180, 87, 198 } },
                    { new Guid("7af6be98-b2f7-3eb4-6aa1-4fa0b8edef41"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("65dbc826-f4a9-8554-e537-0f4d53d8d1f2"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 152, 190, 246, 122, 247, 178, 180, 62, 106, 161, 79, 160, 184, 237, 239, 65 } },
                    { new Guid("7bd050c8-d491-2a7c-eeb3-f2a052daafaa"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("15fe83a8-6390-df6e-0c78-e1c6b3cc183e"), new Guid("4eaf3e0e-0f8e-e7a1-33fb-ebb8807730b8"), new byte[] { 200, 80, 208, 123, 145, 212, 124, 42, 238, 179, 242, 160, 82, 218, 175, 170 } },
                    { new Guid("7cdf28f4-cb11-7867-ac8a-597b1495cd02"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("feb91607-53a4-6736-3e7c-369172310890"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 244, 40, 223, 124, 17, 203, 103, 120, 172, 138, 89, 123, 20, 149, 205, 2 } },
                    { new Guid("7f1416f4-268b-1665-7141-335077bbebf7"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("04fe65fa-64e8-d009-5882-21d829f5816a"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 244, 22, 20, 127, 139, 38, 101, 22, 113, 65, 51, 80, 119, 187, 235, 247 } },
                    { new Guid("835e09a6-f290-50e0-b075-1239eb14de7f"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("e03c8020-f40e-e4f9-b718-204b733a4698"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 166, 9, 94, 131, 144, 242, 224, 80, 176, 117, 18, 57, 235, 20, 222, 127 } },
                    { new Guid("84cdee30-54ed-9057-e563-7c8ae897991b"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("15fe83a8-6390-df6e-0c78-e1c6b3cc183e"), new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"), new byte[] { 48, 238, 205, 132, 237, 84, 87, 144, 229, 99, 124, 138, 232, 151, 153, 27 } },
                    { new Guid("863d260c-c9b9-a8e8-5410-c78009be708a"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("bde05a2b-a0b5-47e8-2b75-9f4b348d69b3"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 12, 38, 61, 134, 185, 201, 232, 168, 84, 16, 199, 128, 9, 190, 112, 138 } },
                    { new Guid("89fde43e-a22d-686f-2033-ef20028b84c4"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("ade50811-4596-96e4-913d-37f34aacd9e6"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 62, 228, 253, 137, 45, 162, 111, 104, 32, 51, 239, 32, 2, 139, 132, 196 } },
                    { new Guid("978d1543-3661-d6bf-7f04-a2b99d01024f"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("cd59d989-9d8f-ab98-fbc6-a06cd3d795a8"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 67, 21, 141, 151, 97, 54, 191, 214, 127, 4, 162, 185, 157, 1, 2, 79 } },
                    { new Guid("97ec44c7-fa6f-0ada-fda9-4e82acf75473"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("757dcc4f-707d-5b9b-3713-59228b0ac543"), new Guid("dbe3bcb2-d68f-c6a7-ae43-5f6acc06c724"), new byte[] { 199, 68, 236, 151, 111, 250, 218, 10, 253, 169, 78, 130, 172, 247, 84, 115 } },
                    { new Guid("9b3816b0-e994-4be3-a61e-077b60eeb62b"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("15fe83a8-6390-df6e-0c78-e1c6b3cc183e"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 176, 22, 56, 155, 148, 233, 227, 75, 166, 30, 7, 123, 96, 238, 182, 43 } },
                    { new Guid("9c098965-7fb6-5850-0ffa-3d741382571d"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("01a7eb08-420e-0140-bd31-f7926fc2d7f8"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 101, 137, 9, 156, 182, 127, 80, 88, 15, 250, 61, 116, 19, 130, 87, 29 } },
                    { new Guid("9dd1bbf0-9bfe-1367-378a-54b99418c22b"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("e3014711-6f98-7517-e57a-4921932d0d88"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 240, 187, 209, 157, 254, 155, 103, 19, 55, 138, 84, 185, 148, 24, 194, 43 } },
                    { new Guid("9df21347-9d9c-cf55-78b2-c4ee2faecff5"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("757dcc4f-707d-5b9b-3713-59228b0ac543"), new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"), new byte[] { 71, 19, 242, 157, 156, 157, 85, 207, 120, 178, 196, 238, 47, 174, 207, 245 } },
                    { new Guid("a1b82771-b639-ceb2-c378-948ac7e66b63"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("bcb6bb17-1916-ddb8-397a-125067962bb2"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 113, 39, 184, 161, 57, 182, 178, 206, 195, 120, 148, 138, 199, 230, 107, 99 } },
                    { new Guid("a938fd72-5808-8c43-c66e-a7651c926c9c"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("31791b9f-c984-1bb4-cbf0-44fcc65bf777"), new Guid("d649856a-053e-1937-04ea-e899c6041d40"), new byte[] { 114, 253, 56, 169, 8, 88, 67, 140, 198, 110, 167, 101, 28, 146, 108, 156 } },
                    { new Guid("ac8571d4-77b2-82e6-b533-e92d3827d4d3"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("2884bbe2-51aa-5948-6df0-95f47b949e78"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 212, 113, 133, 172, 178, 119, 230, 130, 181, 51, 233, 45, 56, 39, 212, 211 } },
                    { new Guid("b8881461-7dea-53c9-2215-1a54b3aeec55"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("31791b9f-c984-1bb4-cbf0-44fcc65bf777"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 97, 20, 136, 184, 234, 125, 201, 83, 34, 21, 26, 84, 179, 174, 236, 85 } },
                    { new Guid("b9fba979-bcbf-988e-a96e-bf823e418173"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("40c239d6-9d6c-903c-9f98-286a13c1110a"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 121, 169, 251, 185, 191, 188, 142, 152, 169, 110, 191, 130, 62, 65, 129, 115 } },
                    { new Guid("bd569804-e9d9-00b1-f5ec-0c83447836b6"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("cd59d989-9d8f-ab98-fbc6-a06cd3d795a8"), new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"), new byte[] { 4, 152, 86, 189, 217, 233, 177, 0, 245, 236, 12, 131, 68, 120, 54, 182 } },
                    { new Guid("bfdbae4d-a680-c91f-18ff-e4c519454cf5"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("680be577-aed0-23bf-c109-0770aa8b4dff"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 77, 174, 219, 191, 128, 166, 31, 201, 24, 255, 228, 197, 25, 69, 76, 245 } },
                    { new Guid("c0ae2f73-4f67-ca2b-dd89-653d5ae11532"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("d5254b68-9969-1be5-3fee-6a30bc62acda"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 115, 47, 174, 192, 103, 79, 43, 202, 221, 137, 101, 61, 90, 225, 21, 50 } },
                    { new Guid("c277160c-9452-e020-117d-dcb1c5b04ee7"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("d011c6b4-f6c0-9416-7ab5-e87242322274"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 12, 22, 119, 194, 82, 148, 32, 224, 17, 125, 220, 177, 197, 176, 78, 231 } },
                    { new Guid("cfc39d54-c374-4a17-109e-ca605d27e1d4"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("bde05a2b-a0b5-47e8-2b75-9f4b348d69b3"), new Guid("4eaf3e0e-0f8e-e7a1-33fb-ebb8807730b8"), new byte[] { 84, 157, 195, 207, 116, 195, 23, 74, 16, 158, 202, 96, 93, 39, 225, 212 } },
                    { new Guid("d9dbff25-efa2-55d6-8657-96e524e598f4"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("2144c9b0-2f6f-077b-320e-300e9934019e"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 37, 255, 219, 217, 162, 239, 214, 85, 134, 87, 150, 229, 36, 229, 152, 244 } },
                    { new Guid("ddc6698c-e550-4d66-46ab-d0779209ea33"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("b5d0f234-0528-f82c-660e-23997e30a690"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 140, 105, 198, 221, 80, 229, 102, 77, 70, 171, 208, 119, 146, 9, 234, 51 } },
                    { new Guid("e0678d26-3c86-5f97-2add-3353a6b7741b"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("cd5ea0fc-cbb0-dcc1-0099-57d7af4aa7c3"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 38, 141, 103, 224, 134, 60, 151, 95, 42, 221, 51, 83, 166, 183, 116, 27 } },
                    { new Guid("e5bd954a-8078-993a-70bd-4b0892983d91"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("d724bd4e-7463-7936-69cf-59d0d5fbdac1"), new Guid("d649856a-053e-1937-04ea-e899c6041d40"), new byte[] { 74, 149, 189, 229, 120, 128, 58, 153, 112, 189, 75, 8, 146, 152, 61, 145 } },
                    { new Guid("e98e3ace-27b1-a374-83f2-349b7914e286"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("12672cb1-68bc-49ce-4ee7-f8155452e26e"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 206, 58, 142, 233, 177, 39, 116, 163, 131, 242, 52, 155, 121, 20, 226, 134 } },
                    { new Guid("e9965e99-224d-cce6-b114-99280032ada2"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("e070e7e3-cee6-0b66-816d-57728e7b2b60"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 153, 94, 150, 233, 77, 34, 230, 204, 177, 20, 153, 40, 0, 50, 173, 162 } },
                    { new Guid("eaca3fc7-6ba3-cdd7-4211-eae37c9b77a7"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("ddc182f7-6225-814c-33ce-9c938b90004d"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 199, 63, 202, 234, 163, 107, 215, 205, 66, 17, 234, 227, 124, 155, 119, 167 } },
                    { new Guid("eb241076-1776-57e0-c564-e8215d721d1b"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("d724bd4e-7463-7936-69cf-59d0d5fbdac1"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 118, 16, 36, 235, 118, 23, 224, 87, 197, 100, 232, 33, 93, 114, 29, 27 } },
                    { new Guid("f2672b3a-9be1-9bca-db36-e1d78edefe07"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("15fe83a8-6390-df6e-0c78-e1c6b3cc183e"), new Guid("dbe3bcb2-d68f-c6a7-ae43-5f6acc06c724"), new byte[] { 58, 43, 103, 242, 225, 155, 202, 155, 219, 54, 225, 215, 142, 222, 254, 7 } },
                    { new Guid("f82df14c-4d8a-6c22-8f30-4246d3fe6eef"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("05261bba-9548-e41f-d1c2-38f94bb71955"), new Guid("4eaf3e0e-0f8e-e7a1-33fb-ebb8807730b8"), new byte[] { 76, 241, 45, 248, 138, 77, 34, 108, 143, 48, 66, 70, 211, 254, 110, 239 } },
                    { new Guid("f91ce31a-65bb-ef07-014a-7f9b793ab0c2"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("04fe65fa-64e8-d009-5882-21d829f5816a"), new Guid("d649856a-053e-1937-04ea-e899c6041d40"), new byte[] { 26, 227, 28, 249, 187, 101, 7, 239, 1, 74, 127, 155, 121, 58, 176, 194 } }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("00a9f22d-def1-4678-7b6b-0e608db9fe0a"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("030b9d75-16ae-6575-5c30-33b08ba7523b"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("073a4315-1e14-e1a9-692f-c7ede437a6d9"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("0762c3b1-94f8-18c1-bc6d-d9a63fd87f66"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("085a54ef-7422-f088-4de5-1259692b6987"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("0aaaacba-ef1c-0ee2-dda2-af44126902a5"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("0bf75260-a7db-7b40-258e-7523b6a9e801"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("0d190790-8659-69ae-2f7a-299ef7133d20"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("10b37995-d61f-06a1-8a65-6cd3b49be0ed"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("123b3178-4b4e-1975-08b1-9c11b4068924"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("12ba1716-0eb2-52a4-adcb-bd0f6a5f9aae"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("1567aa41-8db0-4fe0-36cf-0c800275b8ae"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("18a68d38-0782-208c-f5d7-8ba082c2504c"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("19bc0c4d-bf89-522a-ab35-f23eed07959f"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("1e077679-4755-d044-2538-aaf29da0b707"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("1e8cd71f-2161-8756-213c-11f800fd91eb"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("1f057c3e-d0c9-e27a-7176-fc4a826c7afe"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("2519c4d9-cbd2-4478-bfc6-7e3e426e81f8"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("27df36f7-2b6e-bb94-b25a-ad4ff2dcd8e6"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("2b42fb3a-6553-1e63-0f38-9a4fa575dd70"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("2b5b6ca3-9bf5-5aac-ab8f-722255dc0261"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("2c84f84e-b33c-0fcc-6c3c-3aa1ccf731ee"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("2e584648-5c63-b91c-048b-bb93d6ffa552"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("3078d6ac-2d2b-cb11-1cf8-596332069fc7"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("34f2c8ec-724b-7e78-23f4-e627d0a6be84"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("37230f94-5cbb-79b1-0eb1-d7d447b99ffb"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("3b9436bc-5c42-35fb-472f-44fe95a715c5"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("3be35dd2-d9b7-3a7d-2ec2-9b05d8916a11"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("3f6b711c-7611-8fda-4d07-712d1b6d17ce"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("4ef56edf-20c3-bec8-3171-14acbe517da8"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("50e243dd-c1e3-f5f3-06c8-c751ee515207"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("5268a9ef-0762-7f4c-5d49-e8860be1c733"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("56095114-0eb2-f76b-6703-75e4f2e1659d"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("57c57ec4-6610-536e-c0a7-6c8029f02f72"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("59771783-b148-b636-d09a-010e9d323764"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("5ce0c1c4-2cf3-0d3e-c53b-850396cb5815"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("63da56e2-3904-f2aa-04f5-5b18d66f73e0"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("65f9c188-29bd-ae91-3ed1-1ae10080d650"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("6bb19e2c-1b01-6e58-e9ad-6286dacc70ff"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("723e80dd-a3a7-995d-5b10-f5c8f8b457c6"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("7af6be98-b2f7-3eb4-6aa1-4fa0b8edef41"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("7bd050c8-d491-2a7c-eeb3-f2a052daafaa"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("7cdf28f4-cb11-7867-ac8a-597b1495cd02"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("7f1416f4-268b-1665-7141-335077bbebf7"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("835e09a6-f290-50e0-b075-1239eb14de7f"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("84cdee30-54ed-9057-e563-7c8ae897991b"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("863d260c-c9b9-a8e8-5410-c78009be708a"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("89fde43e-a22d-686f-2033-ef20028b84c4"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("978d1543-3661-d6bf-7f04-a2b99d01024f"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("97ec44c7-fa6f-0ada-fda9-4e82acf75473"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("9b3816b0-e994-4be3-a61e-077b60eeb62b"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("9c098965-7fb6-5850-0ffa-3d741382571d"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("9dd1bbf0-9bfe-1367-378a-54b99418c22b"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("9df21347-9d9c-cf55-78b2-c4ee2faecff5"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("a1b82771-b639-ceb2-c378-948ac7e66b63"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("a938fd72-5808-8c43-c66e-a7651c926c9c"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("ac8571d4-77b2-82e6-b533-e92d3827d4d3"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("b8881461-7dea-53c9-2215-1a54b3aeec55"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("b9fba979-bcbf-988e-a96e-bf823e418173"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("bd569804-e9d9-00b1-f5ec-0c83447836b6"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("bfdbae4d-a680-c91f-18ff-e4c519454cf5"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("c0ae2f73-4f67-ca2b-dd89-653d5ae11532"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("c277160c-9452-e020-117d-dcb1c5b04ee7"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("cfc39d54-c374-4a17-109e-ca605d27e1d4"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("d9dbff25-efa2-55d6-8657-96e524e598f4"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("ddc6698c-e550-4d66-46ab-d0779209ea33"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("e0678d26-3c86-5f97-2add-3353a6b7741b"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("e5bd954a-8078-993a-70bd-4b0892983d91"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("e98e3ace-27b1-a374-83f2-349b7914e286"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("e9965e99-224d-cce6-b114-99280032ada2"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("eaca3fc7-6ba3-cdd7-4211-eae37c9b77a7"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("eb241076-1776-57e0-c564-e8215d721d1b"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("f2672b3a-9be1-9bca-db36-e1d78edefe07"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("f82df14c-4d8a-6c22-8f30-4246d3fe6eef"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("f91ce31a-65bb-ef07-014a-7f9b793ab0c2"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("01a7eb08-420e-0140-bd31-f7926fc2d7f8"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("04fe65fa-64e8-d009-5882-21d829f5816a"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("05261bba-9548-e41f-d1c2-38f94bb71955"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("12672cb1-68bc-49ce-4ee7-f8155452e26e"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("15fe83a8-6390-df6e-0c78-e1c6b3cc183e"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("1765dab6-4360-2b46-b10b-ca6e696fe69f"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("2083e2bb-6cb5-9a66-7eed-95f199125fec"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("2144c9b0-2f6f-077b-320e-300e9934019e"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("2884bbe2-51aa-5948-6df0-95f47b949e78"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("31791b9f-c984-1bb4-cbf0-44fcc65bf777"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("40c239d6-9d6c-903c-9f98-286a13c1110a"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("4fb980f9-59be-aee3-707b-d9d271d683f5"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("65dbc826-f4a9-8554-e537-0f4d53d8d1f2"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("680be577-aed0-23bf-c109-0770aa8b4dff"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("757dcc4f-707d-5b9b-3713-59228b0ac543"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("934ad611-6513-73e9-9310-37b6e2326177"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("a8e8f15d-dd68-a40f-d1f8-66412f5a49cc"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("ade50811-4596-96e4-913d-37f34aacd9e6"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("b5d0f234-0528-f82c-660e-23997e30a690"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("b9b47947-f770-0de9-8be5-ba8b67e42263"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("bc9de91a-026b-febf-57ff-0f580eb84789"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("bcb6bb17-1916-ddb8-397a-125067962bb2"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("bde05a2b-a0b5-47e8-2b75-9f4b348d69b3"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("c69add39-a786-ff31-c160-7fe64e6ca493"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("cd59d989-9d8f-ab98-fbc6-a06cd3d795a8"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("cd5ea0fc-cbb0-dcc1-0099-57d7af4aa7c3"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("d011c6b4-f6c0-9416-7ab5-e87242322274"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("d5254b68-9969-1be5-3fee-6a30bc62acda"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("d724bd4e-7463-7936-69cf-59d0d5fbdac1"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("ddc182f7-6225-814c-33ce-9c938b90004d"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("e03c8020-f40e-e4f9-b718-204b733a4698"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("e070e7e3-cee6-0b66-816d-57728e7b2b60"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("e3014711-6f98-7517-e57a-4921932d0d88"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("f598f262-214c-bf89-fc6a-3ab58d716af5"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("feb91607-53a4-6736-3e7c-369172310890"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("4eaf3e0e-0f8e-e7a1-33fb-ebb8807730b8"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("8fd82f16-6220-b07a-3767-186d8102d6af"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("d649856a-053e-1937-04ea-e899c6041d40"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("dbe3bcb2-d68f-c6a7-ae43-5f6acc06c724"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"));
        }
    }
}
