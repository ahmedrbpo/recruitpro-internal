using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Identity.Entities;

/// <summary>
/// A permission string such as "Recruitment.Job.Publish". Authorization checks are always
/// against this string, never against a role name directly, so an admin can redefine what a
/// role can do without a code change.
/// </summary>
public sealed class Permission : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    private readonly List<RolePermission> _rolePermissions = [];
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    private Permission() { } // EF Core

    public static Permission Create(string name, string? description = null) =>
        new() { Name = name, Description = description };
}
