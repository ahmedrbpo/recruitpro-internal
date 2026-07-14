using FluentAssertions;
using RecruitPro.Domain.Identity.Entities;
using Xunit;

namespace RecruitPro.Domain.Tests.Identity;

public sealed class RoleTests
{
    private static Role CreateRole() => Role.Create("Recruiter", "RECRUITER");

    [Fact]
    public void Deactivate_ThenActivate_RestoresIsActive()
    {
        var role = CreateRole();

        role.Deactivate();
        role.IsActive.Should().BeFalse();

        role.Activate();
        role.IsActive.Should().BeTrue();
    }

    [Fact]
    public void AddPermission_CalledTwiceForSamePermission_IsIdempotent()
    {
        var role = CreateRole();
        var permissionId = Guid.NewGuid();

        role.AddPermission(permissionId);
        role.AddPermission(permissionId);

        role.RolePermissions.Should().ContainSingle(rp => rp.PermissionId == permissionId);
    }

    [Fact]
    public void RemovePermission_RemovesTheAssignment()
    {
        var role = CreateRole();
        var permissionId = Guid.NewGuid();
        role.AddPermission(permissionId);

        role.RemovePermission(permissionId);

        role.RolePermissions.Should().BeEmpty();
    }

    [Fact]
    public void RemovePermission_NotAssigned_IsANoOp()
    {
        var role = CreateRole();

        var act = () => role.RemovePermission(Guid.NewGuid());

        act.Should().NotThrow();
    }
}
