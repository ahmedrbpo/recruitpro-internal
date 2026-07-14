using FluentAssertions;
using RecruitPro.Domain.Identity.Entities;
using Xunit;

namespace RecruitPro.Domain.Tests.Identity;

public sealed class PermissionTests
{
    [Fact]
    public void Deactivate_ThenActivate_RestoresIsActive()
    {
        var permission = Permission.Create("Recruitment.Job.Publish");

        permission.Deactivate();
        permission.IsActive.Should().BeFalse();

        permission.Activate();
        permission.IsActive.Should().BeTrue();
    }
}
