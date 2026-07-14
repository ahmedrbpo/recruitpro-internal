using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Users.AssignRoleToUser;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Identity.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Identity.Users;

public sealed class AssignRoleToUserCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private AssignRoleToUserCommandHandler CreateHandler() => new(_db);

    [Fact]
    public async Task Handle_ValidUserAndRole_AssignsRoleAndReturnsItInDto()
    {
        var user = ApplicationUser.Create("ada@coventine.com", "hash", "Ada");
        var role = Role.Create("Recruiter", "RECRUITER");
        _db.Users.Add(user);
        _db.Roles.Add(role);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new AssignRoleToUserCommand(user.Id, role.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Roles.Should().ContainSingle(r => r.Id == role.Id && r.Name == "Recruiter");
    }

    [Fact]
    public async Task Handle_UnknownUser_ReturnsNotFound()
    {
        var role = Role.Create("Recruiter", "RECRUITER");
        _db.Roles.Add(role);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new AssignRoleToUserCommand(Guid.NewGuid(), role.Id), CancellationToken.None);

        result.Status.Should().Be(ResultStatus.NotFound);
    }

    [Fact]
    public async Task Handle_UnknownRole_ReturnsNotFound()
    {
        var user = ApplicationUser.Create("ada@coventine.com", "hash", "Ada");
        _db.Users.Add(user);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new AssignRoleToUserCommand(user.Id, Guid.NewGuid()), CancellationToken.None);

        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
