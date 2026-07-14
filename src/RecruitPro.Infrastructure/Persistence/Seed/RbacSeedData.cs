using System.Reflection;
using RecruitPro.Domain.Common;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Infrastructure.Persistence.Seed;

/// <summary>
/// Seeds the 6 roles from the RecruitPro Roles &amp; Responsibilities document onto the
/// existing permission catalog — every permission string here already gates a real
/// [RequirePermission] endpoint. The document also references modules with no backend
/// implementation yet (SLA &amp; Delivery Tracking, Placement Management, Onboarding, a formal
/// Job Assignment workflow beyond Job.RecruiterId) — those are intentionally left unmapped since
/// there's nothing for a permission string to gate yet.
/// </summary>
internal static class RbacSeedData
{
    private static readonly DateTimeOffset SeededAt = new(2026, 7, 14, 0, 0, 0, TimeSpan.Zero);

    // Every permission string currently checked by a [RequirePermission] attribute across the API.
    private static readonly string[] AllPermissionNames =
    [
        "Identity.AuditLog.View",
        "Identity.Permission.Manage",
        "Identity.Permission.View",
        "Identity.Role.Manage",
        "Identity.Role.View",
        "Identity.User.Manage",
        "Identity.User.View",
        "Notifications.Log.View",
        "Notifications.Template.Manage",
        "Notifications.Template.View",
        "Recruitment.Application.Create",
        "Recruitment.Application.MoveStage",
        "Recruitment.Application.View",
        "Recruitment.Candidate.Create",
        "Recruitment.Candidate.Update",
        "Recruitment.Candidate.View",
        "Recruitment.Client.Create",
        "Recruitment.Client.View",
        "Recruitment.Department.Create",
        "Recruitment.Department.View",
        "Recruitment.Interview.RecordFeedback",
        "Recruitment.Interview.Schedule",
        "Recruitment.Interview.View",
        "Recruitment.Job.Create",
        "Recruitment.Job.Publish",
        "Recruitment.Job.View",
        "Recruitment.Offer.Create",
        "Recruitment.Offer.Manage",
        "Recruitment.Offer.View",
        "Recruitment.Recruiter.Create",
        "Recruitment.Recruiter.View",
        "Recruitment.Resume.Download",
        "Recruitment.Resume.Upload",
        "Recruitment.Skill.Create",
        "Recruitment.Skill.View",
        "Reporting.Dashboard.View",
    ];

    private sealed record RoleDefinition(string Name, string Code, string Description, bool IsSystem, Guid Id, string[] PermissionNames);

    // A fresh database (a new clone, CI's ephemeral Testcontainers Postgres) has no Recruiter row
    // at all, so this migration must create one like every other role — HasData always inserts,
    // it can't conditionally skip a row. The Id is pinned to the value already live in this
    // project's shared dev database (created ad hoc in an earlier session before this migration
    // existed) purely so that applying this migration there re-establishes the same role identity
    // rather than a different one; see the PR description for the one-time manual reconciliation
    // that database needed (delete + reapply + restore the UserRole assignment).
    private static readonly Guid RecruiterRoleId = Guid.Parse("05921db2-b8f5-462c-9ea5-57ca2d13794e");

    private static readonly RoleDefinition[] Roles =
    [
        new("Administrator", "ADMINISTRATOR",
            "Owns platform governance, security, configuration, and operational administration.",
            IsSystem: true,
            Id: DeterministicGuid.Create("Role.ADMINISTRATOR"),
            PermissionNames: AllPermissionNames),

        new("Delivery Manager", "DELIVERY_MANAGER",
            "Owns delivery execution, resource planning, SLA compliance, and operational performance.",
            IsSystem: false,
            Id: DeterministicGuid.Create("Role.DELIVERY_MANAGER"),
            PermissionNames:
            [
                "Recruitment.Job.View", "Recruitment.Department.View", "Recruitment.Recruiter.View",
                "Recruitment.Application.View", "Recruitment.Interview.View", "Recruitment.Offer.View",
                "Reporting.Dashboard.View",
            ]),

        new("Account Manager", "ACCOUNT_MANAGER",
            "Primary liaison between client and delivery organization.",
            IsSystem: false,
            Id: DeterministicGuid.Create("Role.ACCOUNT_MANAGER"),
            PermissionNames:
            [
                "Recruitment.Client.Create", "Recruitment.Client.View",
                "Recruitment.Job.Create", "Recruitment.Job.Publish", "Recruitment.Job.View",
                "Recruitment.Application.View", "Recruitment.Interview.View",
                "Reporting.Dashboard.View",
            ]),

        new("Team Leader", "TEAM_LEADER",
            "Leads recruiters and ensures timely delivery against assigned requisitions.",
            IsSystem: false,
            Id: DeterministicGuid.Create("Role.TEAM_LEADER"),
            PermissionNames:
            [
                "Recruitment.Job.View", "Recruitment.Recruiter.View", "Recruitment.Candidate.View",
                "Recruitment.Application.View", "Recruitment.Application.MoveStage",
                "Recruitment.Interview.View", "Recruitment.Interview.Schedule", "Recruitment.Interview.RecordFeedback",
                "Reporting.Dashboard.View",
            ]),

        new("Recruiter", "RECRUITER",
            "Sources, evaluates, and submits qualified candidates.",
            IsSystem: false,
            Id: RecruiterRoleId,
            PermissionNames:
            [
                "Recruitment.Job.View",
                "Recruitment.Candidate.Create", "Recruitment.Candidate.Update", "Recruitment.Candidate.View",
                "Recruitment.Resume.Upload", "Recruitment.Resume.Download",
                "Recruitment.Application.Create", "Recruitment.Application.View", "Recruitment.Application.MoveStage",
                "Recruitment.Interview.Schedule", "Recruitment.Interview.View", "Recruitment.Interview.RecordFeedback",
                "Recruitment.Offer.View",
            ]),

        new("Profile Uploader", "PROFILE_UPLOADER",
            "Prepares complete candidate records for recruiters.",
            IsSystem: false,
            Id: DeterministicGuid.Create("Role.PROFILE_UPLOADER"),
            PermissionNames:
            [
                "Recruitment.Candidate.Create", "Recruitment.Candidate.Update", "Recruitment.Candidate.View",
                "Recruitment.Resume.Upload", "Recruitment.Resume.Download",
            ]),
    ];

    public static IReadOnlyCollection<Permission> GetPermissions() =>
        AllPermissionNames.Select(name =>
        {
            var permission = Permission.Create(name);
            Stamp(permission, DeterministicGuid.Create($"Permission.{name}"));
            SetPrivate(permission, "PermissionExtId", DeterministicGuid.Create($"Permission.{name}.ExtId"));
            return permission;
        }).ToList();

    public static IReadOnlyCollection<Role> GetRoles() =>
        Roles.Select(r =>
        {
            var role = Role.Create(r.Name, r.Code, r.Description, r.IsSystem);
            Stamp(role, r.Id);
            SetPrivate(role, "RoleExtId", DeterministicGuid.Create($"Role.{r.Code}.ExtId"));
            return role;
        }).ToList();

    public static IReadOnlyCollection<RolePermission> GetRolePermissions()
    {
        var permissionIdsByName = AllPermissionNames.ToDictionary(name => name, name => DeterministicGuid.Create($"Permission.{name}"));

        return Roles
            .SelectMany(r => r.PermissionNames.Select(permissionName =>
            {
                var rolePermission = RolePermission.Create(r.Id, permissionIdsByName[permissionName]);
                Stamp(rolePermission, DeterministicGuid.Create($"RolePermission.{r.Code}.{permissionName}"));
                return rolePermission;
            }))
            .ToList();
    }

    private static void Stamp(BaseEntity entity, Guid id)
    {
        entity.Id = id;
        entity.CreatedAt = SeededAt;
        entity.RowVersion = id.ToByteArray();
    }

    /// <summary>Sets a private-setter property directly via reflection. Needed only for the
    /// *ExtId fields (RoleExtId/PermissionExtId) — they self-assign Guid.NewGuid() in a field
    /// initializer with no public way to override, but HasData needs every seeded value to be
    /// deterministic (see DeterministicGuid). Nothing about the entities' domain invariants is
    /// bypassed: ExtId is inert metadata not consulted by any business rule today.</summary>
    private static void SetPrivate(object entity, string propertyName, object value)
    {
        var property = entity.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
            ?? throw new InvalidOperationException($"Property '{propertyName}' not found on {entity.GetType().Name}.");
        property.SetValue(entity, value);
    }
}
