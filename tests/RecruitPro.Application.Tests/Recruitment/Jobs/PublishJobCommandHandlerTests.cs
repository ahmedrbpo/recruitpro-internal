using FluentAssertions;
using NSubstitute;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Jobs.PublishJob;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.ValueObjects;
using Xunit;

namespace RecruitPro.Application.Tests.Recruitment.Jobs;

public sealed class PublishJobCommandHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    public PublishJobCommandHandlerTests()
    {
        _dateTimeProvider.UtcNow.Returns(Now);
    }

    private PublishJobCommandHandler CreateHandler() => new(_db, _dateTimeProvider);

    private static Job CreateJob() =>
        Job.Create(
            jobCode: "RP-2026-000001",
            title: "Senior .NET Developer",
            description: "Full job description.",
            employmentType: EmploymentType.FullTime,
            workMode: WorkMode.Remote,
            experienceMin: 3,
            experienceMax: 6,
            currencyCode: "INR");

    [Fact]
    public async Task Handle_JobWithSalaryRange_PublishesSuccessfully()
    {
        var job = CreateJob();
        job.SetSalaryRange(new SalaryRange(1_500_000, 2_200_000));
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new PublishJobCommand(job.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Status.Should().Be(JobStatus.Published);
        result.Value.PublishedDate.Should().Be(DateOnly.FromDateTime(Now.UtcDateTime));
    }

    [Fact]
    public async Task Handle_JobWithSkills_ReturnsSkillsInResponse()
    {
        var job = CreateJob();
        var csharp = Skill.Create("C#");
        var efCore = Skill.Create("EF Core");
        _db.Skills.AddRange(csharp, efCore);
        job.AddSkill(csharp.Id);
        job.AddSkill(efCore.Id);
        job.SetSalaryRange(new SalaryRange(1_500_000, 2_200_000));
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new PublishJobCommand(job.Id), CancellationToken.None);

        result.Value!.Skills.Should().BeEquivalentTo(["C#", "EF Core"]);
    }

    [Fact]
    public async Task Handle_JobWithoutSalaryRange_ThrowsJobMissingSalaryRangeException()
    {
        var job = CreateJob();
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync(CancellationToken.None);

        var act = async () => await CreateHandler().Handle(new PublishJobCommand(job.Id), CancellationToken.None);

        await act.Should().ThrowAsync<JobMissingSalaryRangeException>();
    }

    [Fact]
    public async Task Handle_UnknownJob_ReturnsNotFound()
    {
        var result = await CreateHandler().Handle(new PublishJobCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
