namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>An applicant's expected work arrangement for a specific submission — distinct from
/// <see cref="WorkMode"/>, which is the job requisition's own work mode and has different
/// members (Onsite vs. WorkFromOffice).</summary>
public enum ApplicationWorkType
{
    Remote,
    WorkFromOffice,
    Hybrid,
}
