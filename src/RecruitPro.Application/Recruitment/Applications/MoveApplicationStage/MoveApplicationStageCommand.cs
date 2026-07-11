using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Applications.MoveApplicationStage;

public sealed record MoveApplicationStageCommand(Guid ApplicationId, string NewStage) : IRequest<Result<ApplicationDto>>;
