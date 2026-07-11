namespace RecruitPro.Domain.Common.Exceptions;

public sealed class AccountLockedOutException(DateTimeOffset lockoutEnd)
    : DomainException($"Account is locked out until {lockoutEnd:O}.")
{
    public DateTimeOffset LockoutEnd { get; } = lockoutEnd;
}
