using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Interviews.RescheduleInterview;

public sealed record RescheduleInterviewCommand(Guid InterviewId, DateTimeOffset NewScheduledAt) : IRequest<Result<InterviewDto>>;
