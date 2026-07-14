using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;

namespace RecruitPro.Application.Recruitment.Candidates.ManageEducation;

public sealed class RemoveCandidateEducationCommandHandler(IApplicationDbContext db)
    : IRequestHandler<RemoveCandidateEducationCommand, Result>
{
    public async Task<Result> Handle(RemoveCandidateEducationCommand request, CancellationToken cancellationToken)
    {
        var education = await db.CandidateEducations.SingleOrDefaultAsync(
            e => e.Id == request.EducationId && e.CandidateId == request.CandidateId, cancellationToken);
        if (education is null)
            return Result.NotFound();

        // The SoftDeleteInterceptor converts this Remove() into IsDeleted = true automatically.
        db.CandidateEducations.Remove(education);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
