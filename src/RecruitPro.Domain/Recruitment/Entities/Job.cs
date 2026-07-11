using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.ValueObjects;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class Job : BaseEntity
{
    public string Title { get; private set; } = default!;
    public string Status { get; private set; } = JobStatus.Draft;
    public decimal? SalaryMin { get; private set; }
    public decimal? SalaryMax { get; private set; }
    public Guid? DepartmentId { get; private set; }

    private readonly List<JobSkill> _skills = [];
    public IReadOnlyCollection<JobSkill> Skills => _skills.AsReadOnly();

    private Job() { } // EF Core

    public static Job Create(string title, Guid? departmentId = null) =>
        new() { Title = title, DepartmentId = departmentId };

    public void SetSalaryRange(SalaryRange range)
    {
        SalaryMin = range.Min;
        SalaryMax = range.Max;
    }

    /// <summary>Enforces the invariant that a job must have a salary range before it goes live.</summary>
    public void Publish()
    {
        if (SalaryMin is null || SalaryMax is null)
            throw new JobMissingSalaryRangeException();

        Status = JobStatus.Published;
    }

    public void AddSkill(string skillName)
    {
        if (_skills.Any(s => s.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase)))
            return;

        _skills.Add(JobSkill.Create(Id, skillName));
    }
}
