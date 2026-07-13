using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>Standalone tagging lookup — not yet wired to any entity via a join table since none
/// was specified. Add e.g. a JobTag join table when the first real relationship is needed.</summary>
public sealed class Tag : BaseEntity
{
    public Guid TagExtId { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string? Color { get; private set; }
    public TagCategory Category { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Tag() { } // EF Core

    public static Tag Create(string name, TagCategory category, string? color = null, string? description = null) =>
        new() { Name = name, Category = category, Color = color, Description = description };
}
