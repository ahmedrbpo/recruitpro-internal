using FluentAssertions;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.ValueObjects;
using Xunit;

namespace RecruitPro.Domain.Tests.Recruitment;

public sealed class JobTests
{
    private static readonly DateOnly Today = new(2026, 1, 1);

    private static Job CreateJob(string title = "Senior .NET Developer") =>
        Job.Create(
            jobCode: "RP-2026-000001",
            title: title,
            description: "Full job description.",
            employmentType: EmploymentType.FullTime,
            workMode: WorkMode.Remote,
            experienceMin: 3,
            experienceMax: 6,
            currencyCode: "INR");

    [Fact]
    public void Create_ExperienceMaxBelowMin_ThrowsInvalidExperienceRangeException()
    {
        var act = () => Job.Create(
            "RP-2026-000002", "Title", "Description", EmploymentType.FullTime, WorkMode.Onsite,
            experienceMin: 5, experienceMax: 2, currencyCode: "INR");

        act.Should().Throw<InvalidExperienceRangeException>();
    }

    [Fact]
    public void Publish_WithoutSalaryRange_ThrowsJobMissingSalaryRangeException()
    {
        var job = CreateJob();

        var act = () => job.Publish(Today);

        act.Should().Throw<JobMissingSalaryRangeException>();
        job.Status.Should().Be(JobStatus.Draft);
    }

    [Fact]
    public void Publish_WithSalaryRange_Succeeds()
    {
        var job = CreateJob();
        job.SetSalaryRange(new SalaryRange(1_500_000, 2_200_000));

        job.Publish(Today);

        job.Status.Should().Be(JobStatus.Published);
        job.PublishedDate.Should().Be(Today);
    }

    [Fact]
    public void AddSkill_DuplicateSkillId_IsIgnored()
    {
        var job = CreateJob();
        var skillId = Guid.NewGuid();

        job.AddSkill(skillId);
        job.AddSkill(skillId);

        job.Skills.Should().ContainSingle();
    }
}
