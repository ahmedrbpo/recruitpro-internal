using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Interviews.ScheduleInterview;

public sealed record ScheduleInterviewCommand(
    Guid ApplicationId,
    DateTimeOffset ScheduledAt,
    InterviewMode Mode,
    int Round,
    Guid? InterviewerId,
    int? DurationMinutes,
    string? Notes) : IRequest<Result<InterviewDto>>;
