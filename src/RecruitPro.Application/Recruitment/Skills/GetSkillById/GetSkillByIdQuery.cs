using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Skills.GetSkillById;

public sealed record GetSkillByIdQuery(Guid SkillId) : IRequest<Result<SkillDto>>;
