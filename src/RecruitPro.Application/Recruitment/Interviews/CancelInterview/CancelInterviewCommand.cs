using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Interviews.CancelInterview;

public sealed record CancelInterviewCommand(Guid InterviewId) : IRequest<Result<InterviewDto>>;
