using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Skills.CreateSkill;

public sealed record CreateSkillCommand(string Name, string? Category, string? Description) : IRequest<Result<SkillDto>>;
