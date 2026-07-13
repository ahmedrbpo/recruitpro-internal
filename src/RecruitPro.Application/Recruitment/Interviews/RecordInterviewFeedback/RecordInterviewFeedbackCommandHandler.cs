using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Interviews.RecordInterviewFeedback;

public sealed class RecordInterviewFeedbackCommandHandler(IApplicationDbContext db)
    : IRequestHandler<RecordInterviewFeedbackCommand, Result<InterviewDto>>
{
    public async Task<Result<InterviewDto>> Handle(RecordInterviewFeedbackCommand request, CancellationToken cancellationToken)
    {
        var interview = await db.Interviews.Include(i => i.Feedback)
            .SingleOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);
        if (interview is null)
            return Result<InterviewDto>.NotFound();

        // Throws InterviewStateTransitionException if the interview isn't Completed yet, or
        // InvalidInterviewRatingException if Rating is out of range — both left uncaught by
        // design; ExceptionHandlingMiddleware maps DomainException to a 400 response.
        interview.RecordFeedback(request.InterviewerId, request.Rating, request.Recommendation, request.Comments);

        await db.SaveChangesAsync(cancellationToken);

        return Result<InterviewDto>.Success(InterviewDto.FromEntity(interview));
    }
}
