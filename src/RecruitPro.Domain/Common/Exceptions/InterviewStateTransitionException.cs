namespace RecruitPro.Domain.Common.Exceptions;

public sealed class InterviewStateTransitionException(string action, string reason)
    : DomainException($"Cannot {action} this interview: {reason}");
