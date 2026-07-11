using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Applications.GetPipeline;

public sealed class GetPipelineQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetPipelineQuery, Result<IReadOnlyCollection<ApplicationDto>>>
{
    public async Task<Result<IReadOnlyCollection<ApplicationDto>>> Handle(GetPipelineQuery request, CancellationToken cancellationToken)
    {
        var applications = await db.Applications.AsNoTracking()
            .Include(a => a.StageHistory)
            .Where(a => a.JobId == request.JobId)
            .ToListAsync(cancellationToken);

        IReadOnlyCollection<ApplicationDto> dtos = applications.Select(ApplicationDto.FromEntity).ToList();

        return Result<IReadOnlyCollection<ApplicationDto>>.Success(dtos);
    }
}
