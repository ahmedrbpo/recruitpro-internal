using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Interviews.CompleteInterview;

public sealed record CompleteInterviewCommand(Guid InterviewId) : IRequest<Result<InterviewDto>>;
