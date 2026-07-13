namespace RecruitPro.Domain.Common.Exceptions;

public sealed class InvalidInterviewRatingException(int rating)
    : DomainException($"Interview rating {rating} is out of range; must be between 1 and 5.");
