using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Common.Interfaces;

public interface IJwtTokenService
{
    TimeSpan AccessTokenLifetime { get; }
    TimeSpan RefreshTokenLifetime { get; }

    /// <summary>Permission strings are flattened into JWT claims at issuance so the API never
    /// needs a database round-trip to authorize a request.</summary>
    string GenerateAccessToken(ApplicationUser user, IEnumerable<string> permissions);

    /// <summary>Returns the raw token to hand to the client and the hash to persist.</summary>
    (string RawToken, string TokenHash) GenerateRefreshToken();

    string HashRefreshToken(string rawToken);
}
