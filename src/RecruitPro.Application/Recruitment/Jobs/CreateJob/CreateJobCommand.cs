using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Jobs.CreateJob;

public sealed record CreateJobCommand(
    string JobCode,
    string Title,
    string Description,
    EmploymentType EmploymentType,
    WorkMode WorkMode,
    decimal ExperienceMin,
    decimal ExperienceMax,
    string CurrencyCode,
    Guid? ClientId,
    Guid? JobCategoryId,
    Guid? DepartmentId,
    Guid? RecruiterId,
    decimal? SalaryMin,
    decimal? SalaryMax,
    string? Notes,
    IReadOnlyCollection<string>? Skills) : IRequest<Result<JobDto>>;
