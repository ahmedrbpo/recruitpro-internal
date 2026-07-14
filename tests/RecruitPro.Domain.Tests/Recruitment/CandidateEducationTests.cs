using FluentAssertions;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using Xunit;

namespace RecruitPro.Domain.Tests.Recruitment;

public sealed class CandidateEducationTests
{
    private static readonly Guid CandidateId = Guid.NewGuid();

    [Fact]
    public void Create_EndDateBeforeStartDate_ThrowsInvalidDateRangeException()
    {
        var act = () => CandidateEducation.Create(CandidateId, "MIT", "B.Sc.", "Computer Science",
            EducationType.FullTime, new DateOnly(2020, 6, 1), new DateOnly(2019, 6, 1), "Boston");

        act.Should().Throw<InvalidDateRangeException>();
    }

    [Fact]
    public void Create_NoEndDate_Succeeds()
    {
        var education = CandidateEducation.Create(CandidateId, "MIT", "B.Sc.", "Computer Science",
            EducationType.FullTime, new DateOnly(2020, 6, 1), null, "Boston");

        education.EndDate.Should().BeNull();
    }

    [Fact]
    public void Create_ValidRange_Succeeds()
    {
        var education = CandidateEducation.Create(CandidateId, "MIT", "B.Sc.", "Computer Science",
            EducationType.FullTime, new DateOnly(2016, 6, 1), new DateOnly(2020, 6, 1), "Boston");

        education.CandidateId.Should().Be(CandidateId);
        education.Type.Should().Be(EducationType.FullTime);
    }
}
