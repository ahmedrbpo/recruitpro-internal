using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Events;

public sealed record OfferExtendedEvent(
    Guid OfferId,
    Guid ApplicationId,
    decimal OfferedSalary,
    string CurrencyCode) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
