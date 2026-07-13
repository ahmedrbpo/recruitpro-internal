using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitPro.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJobAddClientAndJobCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "user_roles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "user_roles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "roles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "roles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "role_permissions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "role_permissions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "resumes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "resumes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "refresh_tokens",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "refresh_tokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "permissions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "permissions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "client_id",
                table: "jobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "currency_code",
                table: "jobs",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "jobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "jobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "jobs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "employment_type",
                table: "jobs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "experience_max",
                table: "jobs",
                type: "numeric(4,1)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "experience_min",
                table: "jobs",
                type: "numeric(4,1)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "job_category_id",
                table: "jobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "job_code",
                table: "jobs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "recruiter_id",
                table: "jobs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "work_mode",
                table: "jobs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "job_skills",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "job_skills",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "departments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "departments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "candidates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "candidates",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "applications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "applications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "application_stage_history",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "application_stage_history",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    legal_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    display_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    company_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    client_status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_preferred_client = table.Column<bool>(type: "boolean", nullable: false),
                    is_blacklisted = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("pk_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "job_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
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
                    table.PrimaryKey("pk_job_categories", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_jobs_client_id",
                table: "jobs",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_jobs_job_category_id",
                table: "jobs",
                column: "job_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_jobs_job_code",
                table: "jobs",
                column: "job_code",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_jobs_recruiter_id",
                table: "jobs",
                column: "recruiter_id");

            migrationBuilder.CreateIndex(
                name: "ix_clients_client_code",
                table: "clients",
                column: "client_code",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_clients_client_status",
                table: "clients",
                column: "client_status",
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_job_categories_name",
                table: "job_categories",
                column: "name",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_clients_client_id",
                table: "jobs",
                column: "client_id",
                principalTable: "clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_job_categories_job_category_id",
                table: "jobs",
                column: "job_category_id",
                principalTable: "job_categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_users_recruiter_id",
                table: "jobs",
                column: "recruiter_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_jobs_clients_client_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_job_categories_job_category_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_users_recruiter_id",
                table: "jobs");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "job_categories");

            migrationBuilder.DropIndex(
                name: "ix_jobs_client_id",
                table: "jobs");

            migrationBuilder.DropIndex(
                name: "ix_jobs_job_category_id",
                table: "jobs");

            migrationBuilder.DropIndex(
                name: "ix_jobs_job_code",
                table: "jobs");

            migrationBuilder.DropIndex(
                name: "ix_jobs_recruiter_id",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "users");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "role_permissions");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "role_permissions");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "resumes");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "resumes");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "client_id",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "currency_code",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "description",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "employment_type",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "experience_max",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "experience_min",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "job_category_id",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "job_code",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "recruiter_id",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "work_mode",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "job_skills");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "job_skills");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "candidates");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "application_stage_history");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "application_stage_history");
        }
    }
}
