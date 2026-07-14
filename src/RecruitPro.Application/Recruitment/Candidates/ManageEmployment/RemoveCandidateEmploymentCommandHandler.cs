using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;

namespace RecruitPro.Application.Recruitment.Candidates.ManageEmployment;

public sealed class RemoveCandidateEmploymentCommandHandler(IApplicationDbContext db)
    : IRequestHandler<RemoveCandidateEmploymentCommand, Result>
{
    public async Task<Result> Handle(RemoveCandidateEmploymentCommand request, CancellationToken cancellationToken)
    {
        var employment = await db.CandidateEmploymentHistories.SingleOrDefaultAsync(
            e => e.Id == request.EmploymentId && e.CandidateId == request.CandidateId, cancellationToken);
        if (employment is null)
            return Result.NotFound();

        // The SoftDeleteInterceptor converts this Remove() into IsDeleted = true automatically.
        db.CandidateEmploymentHistories.Remove(employment);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
