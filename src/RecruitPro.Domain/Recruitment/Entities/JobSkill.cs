using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class JobSkill : BaseEntity
{
    public Guid JobId { get; private set; }
    public Job? Job { get; private set; }
    public Guid SkillId { get; private set; }
    public Skill? Skill { get; private set; }

    private JobSkill() { } // EF Core

    public static JobSkill Create(Guid jobId, Guid skillId) => new() { JobId = jobId, SkillId = skillId };
}
