using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;

namespace RecruitPro.Domain.Identity.Entities;

public sealed class ApplicationUser : BaseEntity
{
    private const int MaxFailedAccessAttempts = 5;
    private static readonly TimeSpan BaseLockoutDuration = TimeSpan.FromMinutes(15);

    public string Email { get; private set; } = default!;
    public string NormalizedEmail { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    public int AccessFailedCount { get; private set; }
    public int LockoutCount { get; private set; }
    public DateTimeOffset? LockoutEnd { get; private set; }

    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private readonly List<RefreshToken> _refreshTokens = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private ApplicationUser() { } // EF Core

    public static ApplicationUser Create(string email, string passwordHash, string firstName, string lastName)
    {
        return new ApplicationUser
        {
            Email = email,
            NormalizedEmail = email.Trim().ToUpperInvariant(),
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
        };
    }

    public bool IsLockedOut(DateTimeOffset now) => LockoutEnd is not null && LockoutEnd > now;

    /// <summary>
    /// Failed-attempt lockout follows the blueprint's password policy: 5 failed attempts trigger
    /// a lockout, with exponential backoff (15m, 30m, 60m, ...) on repeated lockouts.
    /// </summary>
    public void RegisterFailedLoginAttempt(DateTimeOffset now)
    {
        if (IsLockedOut(now))
            throw new AccountLockedOutException(LockoutEnd!.Value);

        AccessFailedCount++;
        if (AccessFailedCount < MaxFailedAccessAttempts) return;

        LockoutCount++;
        var backoff = TimeSpan.FromTicks(BaseLockoutDuration.Ticks * (1L << Math.Min(LockoutCount - 1, 8)));
        LockoutEnd = now.Add(backoff);
        AccessFailedCount = 0;
    }

    public void RegisterSuccessfulLogin()
    {
        AccessFailedCount = 0;
        LockoutEnd = null;
    }

    public void ChangePasswordHash(string newPasswordHash) => PasswordHash = newPasswordHash;

    public void Deactivate() => IsActive = false;

    public IEnumerable<string> GetPermissionNames() =>
        UserRoles
            .Select(ur => ur.Role)
            .Where(role => role is not null)
            .SelectMany(role => role!.RolePermissions)
            .Select(rp => rp.Permission)
            .Where(p => p is not null)
            .Select(p => p!.Name)
            .Distinct();
}
