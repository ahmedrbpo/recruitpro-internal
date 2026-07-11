using FluentAssertions;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.ValueObjects;
using Xunit;

namespace RecruitPro.Domain.Tests.Recruitment;

public sealed class JobTests
{
    [Fact]
    public void Publish_WithoutSalaryRange_ThrowsJobMissingSalaryRangeException()
    {
        var job = Job.Create("Senior .NET Developer");

        var act = job.Publish;

        act.Should().Throw<JobMissingSalaryRangeException>();
        job.Status.Should().Be(JobStatus.Draft);
    }

    [Fact]
    public void Publish_WithSalaryRange_Succeeds()
    {
        var job = Job.Create("Senior .NET Developer");
        job.SetSalaryRange(new SalaryRange(1_500_000, 2_200_000));

        job.Publish();

        job.Status.Should().Be(JobStatus.Published);
    }

    [Fact]
    public void AddSkill_DuplicateName_IsIgnored()
    {
        var job = Job.Create("Senior .NET Developer");

        job.AddSkill("C#");
        job.AddSkill("c#");

        job.Skills.Should().ContainSingle();
    }
}
