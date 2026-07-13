using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Jobs.GetJobById;

public sealed class GetJobByIdQueryHandler(IApplicationDbContext db) : IRequestHandler<GetJobByIdQuery, Result<JobDto>>
{
    public async Task<Result<JobDto>> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await db.Jobs.AsNoTracking().Include(j => j.Skills).ThenInclude(js => js.Skill)
            .SingleOrDefaultAsync(j => j.Id == request.JobId, cancellationToken);

        return job is null ? Result<JobDto>.NotFound() : Result<JobDto>.Success(JobDto.FromEntity(job));
    }
}
