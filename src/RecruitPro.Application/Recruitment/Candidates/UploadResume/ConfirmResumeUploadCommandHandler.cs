using MediatR;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Candidates.UploadResume;

public sealed class ConfirmResumeUploadCommandHandler(IApplicationDbContext db, IDateTimeProvider dateTimeProvider)
    : IRequestHandler<ConfirmResumeUploadCommand, Result<ResumeDto>>
{
    public async Task<Result<ResumeDto>> Handle(ConfirmResumeUploadCommand request, CancellationToken cancellationToken)
    {
        var resume = await db.Resumes.FindAsync([request.ResumeId], cancellationToken);
        if (resume is null)
            return Result<ResumeDto>.NotFound();

        resume.Confirm(dateTimeProvider.UtcNow);
        await db.SaveChangesAsync(cancellationToken);

        return Result<ResumeDto>.Success(ResumeDto.FromEntity(resume));
    }
}
