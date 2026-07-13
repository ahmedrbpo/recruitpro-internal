using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record InterviewFeedbackDto(Guid Id, Guid InterviewerId, int Rating, RecommendationType Recommendation, string? Comments)
{
    public static InterviewFeedbackDto FromEntity(InterviewFeedback feedback) =>
        new(feedback.Id, feedback.InterviewerId, feedback.Rating, feedback.Recommendation, feedback.Comments);
}
