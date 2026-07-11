using MediatR;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Jobs.PublishJob;

public sealed class PublishJobCommandHandler(IApplicationDbContext db) : IRequestHandler<PublishJobCommand, Result<JobDto>>
{
    public async Task<Result<JobDto>> Handle(PublishJobCommand request, CancellationToken cancellationToken)
    {
        var job = await db.Jobs.FindAsync([request.JobId], cancellationToken);
        if (job is null)
            return Result<JobDto>.NotFound();

        // Throws JobMissingSalaryRangeException (a DomainException) if the invariant isn't met —
        // left uncaught here by design, per the blueprint's own example. ExceptionHandlingMiddleware
        // maps DomainException to a 400 ProblemDetails response.
        job.Publish();

        await db.SaveChangesAsync(cancellationToken);

        return Result<JobDto>.Success(JobDto.FromEntity(job));
    }
}
