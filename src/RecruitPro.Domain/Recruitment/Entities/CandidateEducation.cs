using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;

namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>One row per education entry on a candidate's profile — a standalone entity (not
/// aggregate-internal), matching how <see cref="Resume"/> is created and persisted directly
/// rather than through a Candidate collection method.</summary>
public sealed class CandidateEducation : BaseEntity
{
    public Guid CandidateId { get; private set; }
    public Candidate? Candidate { get; private set; }
    public string College { get; private set; } = default!;
    public string Degree { get; private set; } = default!;
    public string? Stream { get; private set; }
    public EducationType Type { get; private set; }
    public DateOnly StartDate { get; private set; } // month/year granularity: day fixed to 1
    public DateOnly? EndDate { get; private set; } // null = ongoing
    public string? Location { get; private set; }

    private CandidateEducation() { } // EF Core

    public static CandidateEducation Create(Guid candidateId, string college, string degree, string? stream,
        EducationType type, DateOnly startDate, DateOnly? endDate, string? location)
    {
        if (endDate is not null && endDate < startDate)
            throw new InvalidDateRangeException(startDate, endDate.Value);

        return new CandidateEducation
        {
            CandidateId = candidateId,
            College = college,
            Degree = degree,
            Stream = stream,
            Type = type,
            StartDate = startDate,
            EndDate = endDate,
            Location = location,
        };
    }
}
