using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record InterviewDto(
    Guid Id,
    Guid ApplicationId,
    DateTimeOffset ScheduledAt,
    int? DurationMinutes,
    InterviewMode Mode,
    int Round,
    Guid? InterviewerId,
    InterviewStatus Status,
    string? Notes,
    IReadOnlyCollection<InterviewFeedbackDto> Feedback)
{
    /// <summary>Requires Interview.Feedback to have been loaded (.Include(i => i.Feedback)).</summary>
    public static InterviewDto FromEntity(Interview interview) =>
        new(
            interview.Id,
            interview.ApplicationId,
            interview.ScheduledAt,
            interview.DurationMinutes,
            interview.Mode,
            interview.Round,
            interview.InterviewerId,
            interview.Status,
            interview.Notes,
            interview.Feedback.Select(InterviewFeedbackDto.FromEntity).ToList());
}
