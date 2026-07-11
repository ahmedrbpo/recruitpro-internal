using System.Text.Json.Serialization;

namespace RecruitPro.Application.Identity.Dtos;

/// <summary>
/// RefreshToken is deliberately excluded from JSON serialization: the API controller reads it
/// once to set an httpOnly cookie and must never let it reach the response body, since the
/// access token is the only thing the SPA is meant to hold in memory.
///
/// RefreshToken/RefreshTokenExpiresAt are NOT marked `required`: System.Text.Json rejects a
/// property that is both `required` and [JsonIgnore]'d, since a required member must be
/// deserializable. Every constructor call site still sets all four fields explicitly.
/// </summary>
public sealed class AuthTokensDto
{
    public required string AccessToken { get; init; }
    public required DateTimeOffset AccessTokenExpiresAt { get; init; }

    [JsonIgnore]
    public string RefreshToken { get; init; } = default!;

    [JsonIgnore]
    public DateTimeOffset RefreshTokenExpiresAt { get; init; }
}
