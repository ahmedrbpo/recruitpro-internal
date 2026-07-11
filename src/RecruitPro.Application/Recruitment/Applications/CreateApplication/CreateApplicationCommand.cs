using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Applications.CreateApplication;

public sealed record CreateApplicationCommand(Guid JobId, Guid CandidateId) : IRequest<Result<ApplicationDto>>;
