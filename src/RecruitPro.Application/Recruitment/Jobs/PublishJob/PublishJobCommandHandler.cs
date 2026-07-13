using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Jobs.PublishJob;

public sealed class PublishJobCommandHandler(IApplicationDbContext db, IDateTimeProvider dateTimeProvider)
    : IRequestHandler<PublishJobCommand, Result<JobDto>>
{
    public async Task<Result<JobDto>> Handle(PublishJobCommand request, CancellationToken cancellationToken)
    {
        // Include(Skills), not FindAsync: FindAsync can't eager-load navigations, which left the
        // publish response showing an empty skills array even though skills were persisted fine.
        var job = await db.Jobs.Include(j => j.Skills).ThenInclude(js => js.Skill)
            .SingleOrDefaultAsync(j => j.Id == request.JobId, cancellationToken);
        if (job is null)
            return Result<JobDto>.NotFound();

        // Throws JobMissingSalaryRangeException (a DomainException) if the invariant isn't met —
        // left uncaught here by design, per the blueprint's own example. ExceptionHandlingMiddleware
        // maps DomainException to a 400 ProblemDetails response.
        job.Publish(DateOnly.FromDateTime(dateTimeProvider.UtcNow.UtcDateTime));

        await db.SaveChangesAsync(cancellationToken);

        return Result<JobDto>.Success(JobDto.FromEntity(job));
    }
}
