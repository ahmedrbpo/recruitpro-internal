using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Jobs.GetJobById;

public sealed record GetJobByIdQuery(Guid JobId) : IRequest<Result<JobDto>>;
