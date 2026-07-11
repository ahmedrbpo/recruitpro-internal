using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Identity.Entities;

public sealed class RolePermission : BaseEntity
{
    public Guid RoleId { get; private set; }
    public Role? Role { get; private set; }

    public Guid PermissionId { get; private set; }
    public Permission? Permission { get; private set; }

    private RolePermission() { } // EF Core

    public static RolePermission Create(Guid roleId, Guid permissionId) =>
        new() { RoleId = roleId, PermissionId = permissionId };
}
