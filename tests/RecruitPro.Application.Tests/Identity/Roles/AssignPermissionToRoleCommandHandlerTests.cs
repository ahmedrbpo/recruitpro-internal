using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Roles.AssignPermissionToRole;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Identity.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Identity.Roles;

public sealed class AssignPermissionToRoleCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private AssignPermissionToRoleCommandHandler CreateHandler() => new(_db);

    [Fact]
    public async Task Handle_ValidRoleAndPermission_AssignsPermissionAndReturnsItInDto()
    {
        var role = Role.Create("Recruiter", "RECRUITER");
        var permission = Permission.Create("Recruitment.Job.Publish");
        _db.Roles.Add(role);
        _db.Permissions.Add(permission);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new AssignPermissionToRoleCommand(role.Id, permission.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Permissions.Should().ContainSingle(p => p.Id == permission.Id && p.Name == "Recruitment.Job.Publish");
    }

    [Fact]
    public async Task Handle_UnknownRole_ReturnsNotFound()
    {
        var permission = Permission.Create("Recruitment.Job.Publish");
        _db.Permissions.Add(permission);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new AssignPermissionToRoleCommand(Guid.NewGuid(), permission.Id), CancellationToken.None);

        result.Status.Should().Be(ResultStatus.NotFound);
    }

    [Fact]
    public async Task Handle_UnknownPermission_ReturnsNotFound()
    {
        var role = Role.Create("Recruiter", "RECRUITER");
        _db.Roles.Add(role);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new AssignPermissionToRoleCommand(role.Id, Guid.NewGuid()), CancellationToken.None);

        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
