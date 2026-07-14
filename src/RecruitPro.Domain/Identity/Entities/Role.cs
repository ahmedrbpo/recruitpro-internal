using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Identity.Entities;

public sealed class Role : BaseEntity
{
    public Guid RoleExtId { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsSystem { get; private set; }
    public bool IsActive { get; private set; } = true;

    private readonly List<RolePermission> _rolePermissions = [];
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private Role() { } // EF Core

    public static Role Create(string name, string code, string? description = null, bool isSystem = false) =>
        new() { Name = name, Code = code, Description = description, IsSystem = isSystem };

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void AddPermission(Guid permissionId)
    {
        if (_rolePermissions.Any(rp => rp.PermissionId == permissionId)) return;

        _rolePermissions.Add(RolePermission.Create(Id, permissionId));
    }

    public void RemovePermission(Guid permissionId) => _rolePermissions.RemoveAll(rp => rp.PermissionId == permissionId);
}
