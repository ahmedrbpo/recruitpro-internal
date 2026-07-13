using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Interviews.RescheduleInterview;

public sealed class RescheduleInterviewCommandHandler(IApplicationDbContext db) : IRequestHandler<RescheduleInterviewCommand, Result<InterviewDto>>
{
    public async Task<Result<InterviewDto>> Handle(RescheduleInterviewCommand request, CancellationToken cancellationToken)
    {
        var interview = await db.Interviews.Include(i => i.Feedback)
            .SingleOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);
        if (interview is null)
            return Result<InterviewDto>.NotFound();

        // Throws InterviewStateTransitionException (a DomainException) if not Scheduled — left
        // uncaught by design; ExceptionHandlingMiddleware maps it to a 400 response.
        interview.Reschedule(request.NewScheduledAt);

        await db.SaveChangesAsync(cancellationToken);

        return Result<InterviewDto>.Success(InterviewDto.FromEntity(interview));
    }
}
