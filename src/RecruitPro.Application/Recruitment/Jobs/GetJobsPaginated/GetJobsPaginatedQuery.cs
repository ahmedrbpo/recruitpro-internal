using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Jobs.GetJobsPaginated;

public sealed record GetJobsPaginatedQuery(int Page = 1, int PageSize = 20, string? Status = null)
    : IRequest<Result<PaginatedList<JobDto>>>;
