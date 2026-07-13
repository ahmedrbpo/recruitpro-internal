namespace RecruitPro.Domain.Common.Exceptions;

public sealed class OfferStateTransitionException(string action, string reason)
    : DomainException($"Cannot {action} this offer: {reason}");
