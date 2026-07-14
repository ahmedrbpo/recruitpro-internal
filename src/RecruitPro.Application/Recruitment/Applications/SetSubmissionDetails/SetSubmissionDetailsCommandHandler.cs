using MediatR;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Applications.SetSubmissionDetails;

public sealed class SetSubmissionDetailsCommandHandler(IApplicationDbContext db)
    : IRequestHandler<SetSubmissionDetailsCommand, Result<ApplicationDto>>
{
    public async Task<Result<ApplicationDto>> Handle(SetSubmissionDetailsCommand request, CancellationToken cancellationToken)
    {
        var application = await db.Applications.FindAsync([request.ApplicationId], cancellationToken);
        if (application is null)
            return Result<ApplicationDto>.NotFound();

        application.SetSubmissionDetails(request.WorkType, request.InterviewType,
            request.CurrentCTC, request.ExpectedCTC, request.UANNumber);
        await db.SaveChangesAsync(cancellationToken);

        return Result<ApplicationDto>.Success(ApplicationDto.FromEntity(application));
    }
}
