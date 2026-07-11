using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Identity.Entities;

public sealed class UserRole : BaseEntity
{
    public Guid UserId { get; private set; }
    public ApplicationUser? User { get; private set; }

    public Guid RoleId { get; private set; }
    public Role? Role { get; private set; }

    private UserRole() { } // EF Core

    public static UserRole Create(Guid userId, Guid roleId) =>
        new() { UserId = userId, RoleId = roleId };
}
