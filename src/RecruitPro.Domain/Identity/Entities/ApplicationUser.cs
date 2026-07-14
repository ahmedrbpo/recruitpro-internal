using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Domain.Identity.Entities;

public sealed class ApplicationUser : BaseEntity
{
    private const int MaxFailedAccessAttempts = 5;
    private static readonly TimeSpan BaseLockoutDuration = TimeSpan.FromMinutes(15);

    public Guid UserExtId { get; private set; } = Guid.NewGuid();
    public Guid? DepartmentId { get; private set; }
    public Department? Department { get; private set; }

    public string Email { get; private set; } = default!;
    public string NormalizedEmail { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string? LastName { get; private set; }
    public string? Phone { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool EmailVerified { get; private set; }
    public bool TwoFactorEnabled { get; private set; }
    public DateTimeOffset? LastLoginAt { get; private set; }

    public int AccessFailedCount { get; private set; }
    public int LockoutCount { get; private set; }
    public DateTimeOffset? LockoutEnd { get; private set; }

    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private readonly List<RefreshToken> _refreshTokens = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private ApplicationUser() { } // EF Core

    public static ApplicationUser Create(string email, string passwordHash, string firstName, string? lastName = null, Guid? departmentId = null)
    {
        return new ApplicationUser
        {
            Email = email,
            NormalizedEmail = email.Trim().ToUpperInvariant(),
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            DepartmentId = departmentId,
        };
    }

    public bool IsLockedOut(DateTimeOffset now) => LockoutEnd is not null && LockoutEnd > now;

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

    public void RegisterSuccessfulLogin(DateTimeOffset now)
    {
        AccessFailedCount = 0;
        LockoutEnd = null;
        LastLoginAt = now;
    }

    public void ChangePasswordHash(string newPasswordHash) => PasswordHash = newPasswordHash;

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;

    public void AddRole(Guid roleId)
    {
        if (_userRoles.Any(ur => ur.RoleId == roleId)) return;

        _userRoles.Add(UserRole.Create(Id, roleId));
    }

    public void RemoveRole(Guid roleId) => _userRoles.RemoveAll(ur => ur.RoleId == roleId);

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
