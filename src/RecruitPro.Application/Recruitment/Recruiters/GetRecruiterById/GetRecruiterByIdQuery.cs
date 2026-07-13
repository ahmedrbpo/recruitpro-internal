using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Recruiters.GetRecruiterById;

public sealed record GetRecruiterByIdQuery(Guid RecruiterId) : IRequest<Result<RecruiterDto>>;
