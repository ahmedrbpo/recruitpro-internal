using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class Department : BaseEntity
{
    public string Name { get; private set; } = default!;

    private Department() { } // EF Core

    public static Department Create(string name) => new() { Name = name };
}
