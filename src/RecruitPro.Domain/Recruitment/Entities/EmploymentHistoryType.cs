namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>A candidate's past-job type on their employment history — distinct from
/// <see cref="EmploymentType"/>, which describes a job requisition's offered employment type
/// and has different members (ContractToHire, Internship, Temporary vs. Contract, Freelance).</summary>
public enum EmploymentHistoryType
{
    FullTime,
    PartTime,
    Contract,
    Freelance,
}
