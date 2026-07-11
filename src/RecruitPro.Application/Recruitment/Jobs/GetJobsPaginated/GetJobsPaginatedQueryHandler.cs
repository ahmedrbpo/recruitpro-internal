using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Jobs.GetJobsPaginated;

public sealed class GetJobsPaginatedQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetJobsPaginatedQuery, Result<PaginatedList<JobDto>>>
{
    public async Task<Result<PaginatedList<JobDto>>> Handle(GetJobsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = db.Jobs.AsNoTracking().Include(j => j.Skills).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Status))
            query = query.Where(j => j.Status == request.Status);

        query = query.OrderByDescending(j => j.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);
        var jobs = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);

        var page = new PaginatedList<JobDto>(jobs.Select(JobDto.FromEntity).ToList(), totalCount, request.Page, request.PageSize);

        return Result<PaginatedList<JobDto>>.Success(page);
    }
}
