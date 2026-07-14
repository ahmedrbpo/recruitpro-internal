using FluentAssertions;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using Xunit;

namespace RecruitPro.Domain.Tests.Recruitment;

public sealed class CandidateEmploymentHistoryTests
{
    private static readonly Guid CandidateId = Guid.NewGuid();

    [Fact]
    public void Create_EndDateBeforeStartDate_ThrowsInvalidDateRangeException()
    {
        var act = () => CandidateEmploymentHistory.Create(CandidateId, "Acme Payroll", "Acme Corp", "Engineer",
            EmploymentHistoryType.FullTime, new DateOnly(2022, 6, 1), new DateOnly(2021, 6, 1), "Remote");

        act.Should().Throw<InvalidDateRangeException>();
    }

    [Fact]
    public void Create_NoEndDate_Succeeds()
    {
        var employment = CandidateEmploymentHistory.Create(CandidateId, "Acme Payroll", "Acme Corp", "Engineer",
            EmploymentHistoryType.FullTime, new DateOnly(2022, 6, 1), null, "Remote");

        employment.EndDate.Should().BeNull();
    }

    [Fact]
    public void Create_ValidRange_Succeeds()
    {
        var employment = CandidateEmploymentHistory.Create(CandidateId, "Acme Payroll", "Acme Corp", "Engineer",
            EmploymentHistoryType.Contract, new DateOnly(2020, 1, 1), new DateOnly(2022, 6, 1), "Remote");

        employment.CandidateId.Should().Be(CandidateId);
        employment.Type.Should().Be(EmploymentHistoryType.Contract);
    }
}
