using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class JobSkill : BaseEntity
{
    public Guid JobId { get; private set; }
    public Job? Job { get; private set; }
    public string Name { get; private set; } = default!;

    private JobSkill() { } // EF Core

    public static JobSkill Create(Guid jobId, string name) => new() { JobId = jobId, Name = name };
}
