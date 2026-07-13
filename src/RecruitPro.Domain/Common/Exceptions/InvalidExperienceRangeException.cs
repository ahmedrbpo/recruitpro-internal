namespace RecruitPro.Domain.Common.Exceptions;

public sealed class InvalidExperienceRangeException(decimal min, decimal max)
    : DomainException($"Experience maximum ({max}) must be greater than or equal to the minimum ({min}).");
