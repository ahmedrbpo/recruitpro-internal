using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Jobs.GetJobsPaginated;

public sealed record GetJobsPaginatedQuery(int Page = 1, int PageSize = 20, JobStatus? Status = null)
    : IRequest<Result<PaginatedList<JobDto>>>;
