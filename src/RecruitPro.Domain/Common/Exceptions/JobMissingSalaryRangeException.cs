namespace RecruitPro.Domain.Common.Exceptions;

public sealed class JobMissingSalaryRangeException()
    : DomainException("A job must have a salary range before it can be published.");
