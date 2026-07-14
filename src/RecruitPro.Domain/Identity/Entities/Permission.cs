using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Identity.Entities;

/// <summary>
/// A permission string such as "Recruitment.Job.Publish". Authorization checks are always
/// against Name, never against a role name directly, so an admin can redefine what a role can
/// do without a code change. Resource/Action are descriptive metadata only (e.g. for an admin
/// UI grouping permissions) — not consulted by the authorization pipeline.
/// </summary>
public sealed class Permission : BaseEntity
{
    public Guid PermissionExtId { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string? Resource { get; private set; }
    public PermissionAction? Action { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    private readonly List<RolePermission> _rolePermissions = [];
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    private Permission() { } // EF Core

    public static Permission Create(string name, string? resource = null, PermissionAction? action = null, string? description = null) =>
        new() { Name = name, Resource = resource, Action = action, Description = description };

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
