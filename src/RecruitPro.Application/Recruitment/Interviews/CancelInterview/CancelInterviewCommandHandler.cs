using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Interviews.CancelInterview;

public sealed class CancelInterviewCommandHandler(IApplicationDbContext db) : IRequestHandler<CancelInterviewCommand, Result<InterviewDto>>
{
    public async Task<Result<InterviewDto>> Handle(CancelInterviewCommand request, CancellationToken cancellationToken)
    {
        var interview = await db.Interviews.Include(i => i.Feedback)
            .SingleOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);
        if (interview is null)
            return Result<InterviewDto>.NotFound();

        interview.Cancel();

        await db.SaveChangesAsync(cancellationToken);

        return Result<InterviewDto>.Success(InterviewDto.FromEntity(interview));
    }
}
