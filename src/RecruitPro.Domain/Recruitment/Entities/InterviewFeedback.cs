using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>Child entity of the Interview aggregate — created only via Interview.RecordFeedback(),
/// never directly, so the "interview must be Completed first" invariant always holds.</summary>
public sealed class InterviewFeedback : BaseEntity
{
    private const int MinRating = 1;
    private const int MaxRating = 5;

    public Guid InterviewId { get; private set; }
    public Interview? Interview { get; private set; }
    public Guid InterviewerId { get; private set; }
    public ApplicationUser? Interviewer { get; private set; }
    public int Rating { get; private set; }
    public RecommendationType Recommendation { get; private set; }
    public string? Comments { get; private set; }

    private InterviewFeedback() { } // EF Core

    internal static InterviewFeedback Create(Guid interviewId, Guid interviewerId, int rating, RecommendationType recommendation, string? comments)
    {
        if (rating < MinRating || rating > MaxRating)
            throw new InvalidInterviewRatingException(rating);

        return new InterviewFeedback
        {
            InterviewId = interviewId,
            InterviewerId = interviewerId,
            Rating = rating,
            Recommendation = recommendation,
            Comments = comments,
        };
    }
}
