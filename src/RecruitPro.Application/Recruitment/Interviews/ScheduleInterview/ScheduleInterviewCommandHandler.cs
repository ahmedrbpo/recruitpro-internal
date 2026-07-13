using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Interviews.ScheduleInterview;

public sealed class ScheduleInterviewCommandHandler(IApplicationDbContext db) : IRequestHandler<ScheduleInterviewCommand, Result<InterviewDto>>
{
    public async Task<Result<InterviewDto>> Handle(ScheduleInterviewCommand request, CancellationToken cancellationToken)
    {
        var applicationExists = await db.Applications.AnyAsync(a => a.Id == request.ApplicationId, cancellationToken);
        if (!applicationExists)
            return Result<InterviewDto>.NotFound("Application not found.");

        var interview = Interview.Schedule(
            request.ApplicationId,
            request.ScheduledAt,
            request.Mode,
            request.Round,
            request.InterviewerId,
            request.DurationMinutes,
            request.Notes);

        db.Interviews.Add(interview);
        await db.SaveChangesAsync(cancellationToken);

        return Result<InterviewDto>.Success(InterviewDto.FromEntity(interview));
    }
}
