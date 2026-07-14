using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Applications.GetSubmissionDetails;

public sealed record GetApplicationSubmissionDetailsQuery(Guid ApplicationId) : IRequest<Result<ApplicationSubmissionDetailsDto>>;
