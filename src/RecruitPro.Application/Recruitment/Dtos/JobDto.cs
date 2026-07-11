using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record JobDto(
    Guid Id,
    string Title,
    string Status,
    decimal? SalaryMin,
    decimal? SalaryMax,
    Guid? DepartmentId,
    IReadOnlyCollection<string> Skills)
{
    public static JobDto FromEntity(Job job) =>
        new(job.Id, job.Title, job.Status, job.SalaryMin, job.SalaryMax, job.DepartmentId, job.Skills.Select(s => s.Name).ToList());
}
