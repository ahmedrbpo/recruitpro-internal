using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecruitPro.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCandidateProfileEducationEmploymentAndSubmissionDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "onboarding",
                table: "jobs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "candidates",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "date_of_birth",
                table: "candidates",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "candidates",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pan",
                table: "candidates",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "postal_code",
                table: "candidates",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "relevant_experience_years",
                table: "candidates",
                type: "numeric(4,1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "candidates",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "street_address",
                table: "candidates",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_experience_years",
                table: "candidates",
                type: "numeric(4,1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "work_location",
                table: "candidates",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "current_ctc",
                table: "applications",
                type: "numeric(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "expected_ctc",
                table: "applications",
                type: "numeric(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "interview_type",
                table: "applications",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uan_number",
                table: "applications",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "work_type",
                table: "applications",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "candidate_education",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    candidate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    college = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    degree = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    stream = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_candidate_education", x => x.id);
                    table.ForeignKey(
                        name: "fk_candidate_education_candidates_candidate_id",
                        column: x => x.candidate_id,
                        principalTable: "candidates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "candidate_employment_history",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    candidate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payroll_company = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    company = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    designation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_candidate_employment_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_candidate_employment_history_candidates_candidate_id",
                        column: x => x.candidate_id,
                        principalTable: "candidates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "action", "created_at", "created_by", "deleted_at", "deleted_by", "description", "is_active", "is_deleted", "modified_at", "modified_by", "name", "permission_ext_id", "resource", "row_version" },
                values: new object[] { new Guid("42faef12-2e17-4826-3ec4-203d4d69cb19"), null, new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, true, false, null, null, "Recruitment.Candidate.Update", new Guid("b1a0eded-fab7-c676-9b81-8df3f799b11a"), null, new byte[] { 18, 239, 250, 66, 23, 46, 38, 72, 62, 196, 32, 61, 77, 105, 203, 25 } });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "id", "created_at", "created_by", "deleted_at", "deleted_by", "is_deleted", "modified_at", "modified_by", "permission_id", "role_id", "row_version" },
                values: new object[,]
                {
                    { new Guid("17018b28-9f4a-45c0-4ec2-141a8fac2775"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("42faef12-2e17-4826-3ec4-203d4d69cb19"), new Guid("d649856a-053e-1937-04ea-e899c6041d40"), new byte[] { 40, 139, 1, 23, 74, 159, 192, 69, 78, 194, 20, 26, 143, 172, 39, 117 } },
                    { new Guid("9dd892ae-0857-456d-79e6-d5d2c667d1d9"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("42faef12-2e17-4826-3ec4-203d4d69cb19"), new Guid("e0ab9763-b8e7-dbfc-f47a-0c0824390bfd"), new byte[] { 174, 146, 216, 157, 87, 8, 109, 69, 121, 230, 213, 210, 198, 103, 209, 217 } },
                    { new Guid("aa211b9c-a86a-058c-8af4-bbed8b508b00"), new DateTimeOffset(new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, false, null, null, new Guid("42faef12-2e17-4826-3ec4-203d4d69cb19"), new Guid("05921db2-b8f5-462c-9ea5-57ca2d13794e"), new byte[] { 156, 27, 33, 170, 106, 168, 140, 5, 138, 244, 187, 237, 139, 80, 139, 0 } }
                });

            migrationBuilder.CreateIndex(
                name: "ix_candidate_education_candidate_id",
                table: "candidate_education",
                column: "candidate_id");

            migrationBuilder.CreateIndex(
                name: "ix_candidate_employment_history_candidate_id",
                table: "candidate_employment_history",
                column: "candidate_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "candidate_education");

            migrationBuilder.DropTable(
                name: "candidate_employment_history");

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("17018b28-9f4a-45c0-4ec2-141a8fac2775"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("9dd892ae-0857-456d-79e6-d5d2c667d1d9"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: new Guid("aa211b9c-a86a-058c-8af4-bbed8b508b00"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("42faef12-2e17-4826-3ec4-203d4d69cb19"));

            migrationBuilder.DropColumn(
                name: "onboarding",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "city",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "pan",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "postal_code",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "relevant_experience_years",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "state",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "street_address",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "total_experience_years",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "work_location",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "current_ctc",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "expected_ctc",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "interview_type",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "uan_number",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "work_type",
                table: "applications");
        }
    }
}
