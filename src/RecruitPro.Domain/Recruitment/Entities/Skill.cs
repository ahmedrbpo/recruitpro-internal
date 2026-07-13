using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class Skill : BaseEntity
{
    public Guid SkillExtId { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string? Category { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Skill() { } // EF Core

    public static Skill Create(string name, string? category = null, string? description = null) =>
        new() { Name = name, Category = category, Description = description };
}
