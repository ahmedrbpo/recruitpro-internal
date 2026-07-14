namespace RecruitPro.Domain.Common.Exceptions;

public sealed class InvalidDateRangeException(DateOnly start, DateOnly end)
    : DomainException($"End date ({end}) must be on or after the start date ({start}).");
