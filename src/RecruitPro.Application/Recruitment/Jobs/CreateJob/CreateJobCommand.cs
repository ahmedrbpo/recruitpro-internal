using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Jobs.CreateJob;

public sealed record CreateJobCommand(
    string Title,
    Guid? DepartmentId,
    decimal? SalaryMin,
    decimal? SalaryMax,
    IReadOnlyCollection<string>? Skills) : IRequest<Result<JobDto>>;
