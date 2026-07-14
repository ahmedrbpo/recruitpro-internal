using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Roles.CreateRole;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Identity.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Identity.Roles;

public sealed class CreateRoleCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private CreateRoleCommandHandler CreateHandler() => new(_db);

    [Fact]
    public async Task Handle_NewNameAndCode_CreatesRole()
    {
        var result = await CreateHandler().Handle(new CreateRoleCommand("Recruiter", "RECRUITER", "Manages the pipeline"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.IsSystem.Should().BeFalse();
        _db.Roles.Should().ContainSingle();
    }

    [Fact]
    public async Task Handle_NameAlreadyTaken_ReturnsConflict()
    {
        _db.Roles.Add(Role.Create("Recruiter", "RECRUITER_1"));
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new CreateRoleCommand("Recruiter", "RECRUITER_2", null), CancellationToken.None);

        result.Status.Should().Be(ResultStatus.Conflict);
    }

    [Fact]
    public async Task Handle_CodeAlreadyTaken_ReturnsConflict()
    {
        _db.Roles.Add(Role.Create("Recruiter", "RECRUITER"));
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new CreateRoleCommand("Senior Recruiter", "RECRUITER", null), CancellationToken.None);

        result.Status.Should().Be(ResultStatus.Conflict);
    }
}
