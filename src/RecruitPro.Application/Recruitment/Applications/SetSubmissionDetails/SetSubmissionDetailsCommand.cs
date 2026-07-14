using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Applications.SetSubmissionDetails;

public sealed record SetSubmissionDetailsCommand(
    Guid ApplicationId,
    ApplicationWorkType WorkType,
    ApplicationInterviewType InterviewType,
    decimal CurrentCTC,
    decimal ExpectedCTC,
    string? UANNumber) : IRequest<Result<ApplicationDto>>;
