using MediatR;
using RecruitPro.Domain.Common;

namespace RecruitPro.Application.Common.Models;

/// <summary>Wraps a Domain-layer IDomainEvent so it can be published through MediatR without
/// Domain itself depending on MediatR (Domain has zero dependencies per the architecture rules).
/// Infrastructure's DomainEventDispatchInterceptor constructs the closed generic type via
/// reflection for each concrete event so INotificationHandler&lt;DomainEventNotification&lt;TEvent&gt;&gt;
/// resolves correctly.</summary>
public sealed class DomainEventNotification<TDomainEvent>(TDomainEvent domainEvent) : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; } = domainEvent;
}
