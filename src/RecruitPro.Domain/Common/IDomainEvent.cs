namespace RecruitPro.Domain.Common;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
}
