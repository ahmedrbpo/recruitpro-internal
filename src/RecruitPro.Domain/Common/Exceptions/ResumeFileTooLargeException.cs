namespace RecruitPro.Domain.Common.Exceptions;

public sealed class ResumeFileTooLargeException(long actualBytes, long maxBytes)
    : DomainException($"Resume file size {actualBytes:N0} bytes exceeds the {maxBytes:N0} byte limit.");
