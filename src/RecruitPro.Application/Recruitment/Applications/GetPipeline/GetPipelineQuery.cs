using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Applications.GetPipeline;

public sealed record GetPipelineQuery(Guid JobId) : IRequest<Result<IReadOnlyCollection<ApplicationDto>>>;
