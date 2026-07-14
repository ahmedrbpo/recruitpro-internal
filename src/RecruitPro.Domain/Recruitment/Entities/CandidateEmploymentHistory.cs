using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;

namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>One row per past-employment entry on a candidate's profile — a standalone entity
/// (not aggregate-internal), matching how <see cref="Resume"/> is created and persisted
/// directly rather than through a Candidate collection method.</summary>
public sealed class CandidateEmploymentHistory : BaseEntity
{
    public Guid CandidateId { get; private set; }
    public Candidate? Candidate { get; private set; }
    public string PayrollCompany { get; private set; } = default!;
    public string Company { get; private set; } = default!;
    public string Designation { get; private set; } = default!;
    public EmploymentHistoryType Type { get; private set; }
    public DateOnly StartDate { get; private set; } // month/year granularity: day fixed to 1
    public DateOnly? EndDate { get; private set; } // null = ongoing
    public string? Location { get; private set; }

    private CandidateEmploymentHistory() { } // EF Core

    public static CandidateEmploymentHistory Create(Guid candidateId, string payrollCompany, string company,
        string designation, EmploymentHistoryType type, DateOnly startDate, DateOnly? endDate, string? location)
    {
        if (endDate is not null && endDate < startDate)
            throw new InvalidDateRangeException(startDate, endDate.Value);

        return new CandidateEmploymentHistory
        {
            CandidateId = candidateId,
            PayrollCompany = payrollCompany,
            Company = company,
            Designation = designation,
            Type = type,
            StartDate = startDate,
            EndDate = endDate,
            Location = location,
        };
    }
}
