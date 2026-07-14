using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record JobDto(
    Guid Id,
    string JobCode,
    string Title,
    string Description,
    JobStatus Status,
    EmploymentType EmploymentType,
    WorkMode WorkMode,
    decimal ExperienceMin,
    decimal ExperienceMax,
    decimal? SalaryMin,
    decimal? SalaryMax,
    string CurrencyCode,
    DateOnly? PublishedDate,
    string? Notes,
    OnboardingType? Onboarding,
    Guid? ClientId,
    Guid? JobCategoryId,
    Guid? DepartmentId,
    Guid? RecruiterId,
    IReadOnlyCollection<string> Skills)
{
    /// <summary>Requires Job.Skills to have been loaded with .ThenInclude(js => js.Skill).</summary>
    public static JobDto FromEntity(Job job) =>
        new(
            job.Id,
            job.JobCode,
            job.Title,
            job.Description,
            job.Status,
            job.EmploymentType,
            job.WorkMode,
            job.ExperienceMin,
            job.ExperienceMax,
            job.SalaryMin,
            job.SalaryMax,
            job.CurrencyCode,
            job.PublishedDate,
            job.Notes,
            job.Onboarding,
            job.ClientId,
            job.JobCategoryId,
            job.DepartmentId,
            job.RecruiterId,
            job.Skills.Where(s => s.Skill is not null).Select(s => s.Skill!.Name).ToList());
}
