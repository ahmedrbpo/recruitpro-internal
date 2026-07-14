using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Jobs.SetOnboarding;

public sealed class SetOnboardingCommandHandler(IApplicationDbContext db)
    : IRequestHandler<SetOnboardingCommand, Result<JobDto>>
{
    public async Task<Result<JobDto>> Handle(SetOnboardingCommand request, CancellationToken cancellationToken)
    {
        var job = await db.Jobs.Include(j => j.Skills).ThenInclude(js => js.Skill)
            .SingleOrDefaultAsync(j => j.Id == request.JobId, cancellationToken);
        if (job is null)
            return Result<JobDto>.NotFound();

        job.SetOnboarding(request.Onboarding);
        await db.SaveChangesAsync(cancellationToken);

        return Result<JobDto>.Success(JobDto.FromEntity(job));
    }
}
