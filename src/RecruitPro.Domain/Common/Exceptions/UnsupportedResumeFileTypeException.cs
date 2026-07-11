namespace RecruitPro.Domain.Common.Exceptions;

public sealed class UnsupportedResumeFileTypeException(string contentType)
    : DomainException($"'{contentType}' is not an accepted resume file type. Allowed: PDF, DOC, DOCX.");
