using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Interviews.GetInterviewsForApplication;

public sealed record GetInterviewsForApplicationQuery(Guid ApplicationId) : IRequest<Result<IReadOnlyCollection<InterviewDto>>>;
