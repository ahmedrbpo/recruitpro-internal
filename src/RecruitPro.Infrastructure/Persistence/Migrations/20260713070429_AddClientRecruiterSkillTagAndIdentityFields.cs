using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitPro.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddClientRecruiterSkillTagAndIdentityFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_jobs_users_recruiter_id",
                table: "jobs");

            migrationBuilder.DropIndex(
                name: "ix_job_skills_job_id_name",
                table: "job_skills");

            migrationBuilder.DropIndex(
                name: "ix_clients_client_code",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "ix_clients_client_status",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "name",
                table: "job_skills");

            migrationBuilder.DropColumn(
                name: "client_code",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "display_name",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "is_blacklisted",
                table: "clients");

            migrationBuilder.RenameColumn(
                name: "legal_name",
                table: "clients",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "is_preferred_client",
                table: "clients",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "company_type",
                table: "clients",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "client_status",
                table: "clients",
                newName: "code");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<Guid>(
                name: "department_id",
                table: "users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "email_verified",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "last_login_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "two_factor_enabled",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "user_ext_id",
                table: "users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "roles",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_system",
                table: "roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "role_ext_id",
                table: "roles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "action",
                table: "permissions",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "permissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "permission_ext_id",
                table: "permissions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "resource",
                table: "permissions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "jobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "published_date",
                table: "jobs",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "skill_id",
                table: "job_skills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "job_categories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "job_categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "department_code",
                table: "departments",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "department_ext_id",
                table: "departments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "departments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "departments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "manager_id",
                table: "departments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "parent_department_id",
                table: "departments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "clients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "client_ext_id",
                table: "clients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "clients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "currency_code",
                table: "clients",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "clients",
                type: "character varying(320)",
                maxLength: 320,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gst_number",
                table: "clients",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "hq_country",
                table: "clients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "industry",
                table: "clients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "clients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "clients",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "postal_code",
                table: "clients",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "clients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "website",
                table: "clients",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "action",
                table: "audit_logs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "recruiters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    recruiter_ext_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    vendor_company = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    pan = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    mobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    postal_code = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("pk_recruiters", x => x.id);
                    table.ForeignKey(
                        name: "fk_recruiters_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "skills",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    skill_ext_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("pk_skills", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_ext_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    category = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("pk_tags", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_department_id",
                table: "users",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_user_ext_id",
                table: "users",
                column: "user_ext_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_roles_code",
                table: "roles",
                column: "code",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_roles_role_ext_id",
                table: "roles",
                column: "role_ext_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_permissions_permission_ext_id",
                table: "permissions",
                column: "permission_ext_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_job_skills_job_id_skill_id",
                table: "job_skills",
                columns: new[] { "job_id", "skill_id" },
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_job_skills_skill_id",
                table: "job_skills",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_departments_department_ext_id",
                table: "departments",
                column: "department_ext_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_departments_manager_id",
                table: "departments",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "ix_departments_parent_department_id",
                table: "departments",
                column: "parent_department_id");

            migrationBuilder.CreateIndex(
                name: "ix_clients_client_ext_id",
                table: "clients",
                column: "client_ext_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clients_code",
                table: "clients",
                column: "code",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_clients_is_active",
                table: "clients",
                column: "is_active",
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_recruiters_user_id",
                table: "recruiters",
                column: "user_id",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_skills_name",
                table: "skills",
                column: "name",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_tags_name_category",
                table: "tags",
                columns: new[] { "name", "category" },
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.AddForeignKey(
                name: "fk_departments_departments_parent_department_id",
                table: "departments",
                column: "parent_department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_departments_users_manager_id",
                table: "departments",
                column: "manager_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_job_skills_skills_skill_id",
                table: "job_skills",
                column: "skill_id",
                principalTable: "skills",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_recruiters_recruiter_id",
                table: "jobs",
                column: "recruiter_id",
                principalTable: "recruiters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_users_departments_department_id",
                table: "users",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_departments_departments_parent_department_id",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "fk_departments_users_manager_id",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "fk_job_skills_skills_skill_id",
                table: "job_skills");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_recruiters_recruiter_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_users_departments_department_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "recruiters");

            migrationBuilder.DropTable(
                name: "skills");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropIndex(
                name: "ix_users_department_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_user_ext_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_roles_code",
                table: "roles");

            migrationBuilder.DropIndex(
                name: "ix_roles_role_ext_id",
                table: "roles");

            migrationBuilder.DropIndex(
                name: "ix_permissions_permission_ext_id",
                table: "permissions");

            migrationBuilder.DropIndex(
                name: "ix_job_skills_job_id_skill_id",
                table: "job_skills");

            migrationBuilder.DropIndex(
                name: "ix_job_skills_skill_id",
                table: "job_skills");

            migrationBuilder.DropIndex(
                name: "ix_departments_department_ext_id",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "ix_departments_manager_id",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "ix_departments_parent_department_id",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "ix_clients_client_ext_id",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "ix_clients_code",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "ix_clients_is_active",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "department_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "email_verified",
                table: "users");

            migrationBuilder.DropColumn(
                name: "last_login_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "users");

            migrationBuilder.DropColumn(
                name: "two_factor_enabled",
                table: "users");

            migrationBuilder.DropColumn(
                name: "user_ext_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "code",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "is_system",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "role_ext_id",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "action",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "permission_ext_id",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "resource",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "published_date",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "skill_id",
                table: "job_skills");

            migrationBuilder.DropColumn(
                name: "description",
                table: "job_categories");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "job_categories");

            migrationBuilder.DropColumn(
                name: "department_code",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "department_ext_id",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "description",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "manager_id",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "parent_department_id",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "city",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "client_ext_id",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "country",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "currency_code",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "email",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "gst_number",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "hq_country",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "industry",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "postal_code",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "state",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "website",
                table: "clients");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "clients",
                newName: "company_type");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "clients",
                newName: "legal_name");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "clients",
                newName: "is_preferred_client");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "clients",
                newName: "client_status");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "job_skills",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "client_code",
                table: "clients",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "display_name",
                table: "clients",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_blacklisted",
                table: "clients",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "action",
                table: "audit_logs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.CreateIndex(
                name: "ix_job_skills_job_id_name",
                table: "job_skills",
                columns: new[] { "job_id", "name" },
                unique: true,
                filter: "is_deleted = false");

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

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_users_recruiter_id",
                table: "jobs",
                column: "recruiter_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
