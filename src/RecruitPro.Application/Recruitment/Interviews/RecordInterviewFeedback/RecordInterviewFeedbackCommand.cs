using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Interviews.RecordInterviewFeedback;

public sealed record RecordInterviewFeedbackCommand(
    Guid InterviewId,
    Guid InterviewerId,
    int Rating,
    RecommendationType Recommendation,
    string? Comments) : IRequest<Result<InterviewDto>>;
