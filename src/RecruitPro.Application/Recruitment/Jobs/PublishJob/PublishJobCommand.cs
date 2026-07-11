using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Jobs.PublishJob;

public sealed record PublishJobCommand(Guid JobId) : IRequest<Result<JobDto>>;
