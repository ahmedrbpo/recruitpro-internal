using FluentAssertions;
using RecruitPro.Application.Recruitment.Jobs.CreateJob;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Recruitment.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Recruitment.Jobs;

public sealed class CreateJobCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private static CreateJobCommand BuildCommand(IReadOnlyCollection<string>? skills) => new(
        JobCode: "RP-2026-000001",
        Title: "Senior .NET Developer",
        Description: "Full job description.",
        EmploymentType: EmploymentType.FullTime,
        WorkMode: WorkMode.Remote,
        ExperienceMin: 3,
        ExperienceMax: 6,
        CurrencyCode: "INR",
        ClientId: null,
        JobCategoryId: null,
        DepartmentId: null,
        RecruiterId: null,
        SalaryMin: null,
        SalaryMax: null,
        Notes: null,
        Skills: skills);

    [Fact]
    public async Task Handle_MixOfExistingAndNewSkillNames_ReturnsBothInResponse()
    {
        // Regression test: resolving a *pre-existing* skill via a scalar Id projection left its
        // navigation unpopulated, so the create response silently dropped it (only newly-created
        // skills showed up) even though the database link was correct — caught via live verification.
        _db.Skills.Add(Skill.Create("C#"));
        await _db.SaveChangesAsync(CancellationToken.None);

        var handler = new CreateJobCommandHandler(_db);
        var result = await handler.Handle(BuildCommand(["C#", "EF Core"]), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Skills.Should().BeEquivalentTo(["C#", "EF Core"]);
    }

    [Fact]
    public async Task Handle_ExistingSkillNameCaseInsensitive_ReusesSkillWithoutDuplicating()
    {
        _db.Skills.Add(Skill.Create("C#"));
        await _db.SaveChangesAsync(CancellationToken.None);

        var handler = new CreateJobCommandHandler(_db);
        await handler.Handle(BuildCommand(["c#"]), CancellationToken.None);

        _db.Skills.Count().Should().Be(1);
    }
}
