using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.ValueObjects;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class Job : BaseEntity
{
    public string JobCode { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public JobStatus Status { get; private set; } = JobStatus.Draft;
    public EmploymentType EmploymentType { get; private set; }
    public WorkMode WorkMode { get; private set; }
    public decimal ExperienceMin { get; private set; }
    public decimal ExperienceMax { get; private set; }
    public decimal? SalaryMin { get; private set; }
    public decimal? SalaryMax { get; private set; }
    public string CurrencyCode { get; private set; } = default!;
    public DateOnly? PublishedDate { get; private set; }
    public string? Notes { get; private set; }
    public OnboardingType? Onboarding { get; private set; }

    public Guid? ClientId { get; private set; }
    public Guid? JobCategoryId { get; private set; }
    public Guid? DepartmentId { get; private set; }
    public Guid? RecruiterId { get; private set; }

    private readonly List<JobSkill> _skills = [];
    public IReadOnlyCollection<JobSkill> Skills => _skills.AsReadOnly();

    private Job() { } // EF Core

    public static Job Create(
        string jobCode,
        string title,
        string description,
        EmploymentType employmentType,
        WorkMode workMode,
        decimal experienceMin,
        decimal experienceMax,
        string currencyCode,
        Guid? clientId = null,
        Guid? jobCategoryId = null,
        Guid? departmentId = null,
        Guid? recruiterId = null,
        string? notes = null)
    {
        if (experienceMax < experienceMin)
            throw new InvalidExperienceRangeException(experienceMin, experienceMax);

        return new Job
        {
            JobCode = jobCode,
            Title = title,
            Description = description,
            EmploymentType = employmentType,
            WorkMode = workMode,
            ExperienceMin = experienceMin,
            ExperienceMax = experienceMax,
            CurrencyCode = currencyCode,
            ClientId = clientId,
            JobCategoryId = jobCategoryId,
            DepartmentId = departmentId,
            RecruiterId = recruiterId,
            Notes = notes,
        };
    }

    public void SetSalaryRange(SalaryRange range)
    {
        SalaryMin = range.Min;
        SalaryMax = range.Max;
    }

    public void SetOnboarding(OnboardingType onboarding)
    {
        Onboarding = onboarding;
    }

    /// <summary>Enforces the invariant that a job must have a salary range before it goes live.</summary>
    public void Publish(DateOnly publishedDate)
    {
        if (SalaryMin is null || SalaryMax is null)
            throw new JobMissingSalaryRangeException();

        Status = JobStatus.Published;
        PublishedDate = publishedDate;
    }

    public void AddSkill(Guid skillId)
    {
        if (_skills.Any(s => s.SkillId == skillId))
            return;

        _skills.Add(JobSkill.Create(Id, skillId));
    }
}
