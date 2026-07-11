using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Identity.Entities;

public sealed class Role : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    private readonly List<RolePermission> _rolePermissions = [];
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private Role() { } // EF Core

    public static Role Create(string name, string? description = null) =>
        new() { Name = name, Description = description };
}
