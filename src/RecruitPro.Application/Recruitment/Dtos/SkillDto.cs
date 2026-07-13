using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record SkillDto(Guid Id, string Name, string? Category, string? Description, bool IsActive)
{
    public static SkillDto FromEntity(Skill skill) =>
        new(skill.Id, skill.Name, skill.Category, skill.Description, skill.IsActive);
}
