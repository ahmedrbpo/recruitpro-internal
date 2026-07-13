using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>Lookup entity (e.g. IT, Healthcare, Finance).</summary>
public sealed class JobCategory : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    private JobCategory() { } // EF Core

    public static JobCategory Create(string name, string? description = null) => new() { Name = name, Description = description };
}
