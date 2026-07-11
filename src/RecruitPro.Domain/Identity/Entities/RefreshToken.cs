using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Identity.Entities;

/// <summary>
/// Stored hashed, never in plaintext. Rotation on every refresh plus reuse detection: if a
/// token that was already rotated (ReplacedByTokenHash is set) is presented again, the caller
/// should treat it as a compromise signal and revoke the user's whole session chain.
/// </summary>
public sealed class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }
    public ApplicationUser? User { get; private set; }

    public string TokenHash { get; private set; } = default!;
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }

    private RefreshToken() { } // EF Core

    public static RefreshToken Create(Guid userId, string tokenHash, DateTimeOffset expiresAt) =>
        new()
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAt = expiresAt,
        };

    public bool IsExpired(DateTimeOffset now) => now >= ExpiresAt;
    public bool IsRevoked => RevokedAt is not null;
    public bool IsActive(DateTimeOffset now) => !IsRevoked && !IsExpired(now);

    public void Revoke(DateTimeOffset now, string? replacedByTokenHash = null)
    {
        RevokedAt = now;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}
