using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;
// Aliased because this file lives in the "...Commands.RefreshToken" namespace, which would
// otherwise shadow the unqualified "RefreshToken" entity type name.
using RefreshTokenEntity = RecruitPro.Domain.Identity.Entities.RefreshToken;

namespace RecruitPro.Application.Identity.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IApplicationDbContext db,
    IJwtTokenService jwtTokenService,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<RefreshTokenCommand, Result<AuthTokensDto>>
{
    public async Task<Result<AuthTokensDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = jwtTokenService.HashRefreshToken(request.RawRefreshToken);
        var now = dateTimeProvider.UtcNow;

        var token = await db.RefreshTokens
            .Include(t => t.User)
                .ThenInclude(u => u!.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r!.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
            .SingleOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

        if (token is null)
            return Result<AuthTokensDto>.Unauthorized("Invalid refresh token.");

        if (token.IsRevoked)
        {
            // Reuse of an already-rotated token is a compromise signal: revoke every active
            // token for this user so all sessions are forced to re-authenticate.
            var activeTokens = await db.RefreshTokens
                .Where(t => t.UserId == token.UserId && t.RevokedAt == null)
                .ToListAsync(cancellationToken);

            foreach (var activeToken in activeTokens)
                activeToken.Revoke(now);

            await db.SaveChangesAsync(cancellationToken);
            return Result<AuthTokensDto>.Unauthorized("Refresh token reuse detected; all sessions have been revoked.");
        }

        if (token.IsExpired(now))
            return Result<AuthTokensDto>.Unauthorized("Refresh token has expired.");

        var user = token.User!;
        if (!user.IsActive)
            return Result<AuthTokensDto>.Unauthorized("Account is inactive.");

        var (newRawToken, newTokenHash) = jwtTokenService.GenerateRefreshToken();
        var newExpiresAt = now.Add(jwtTokenService.RefreshTokenLifetime);

        token.Revoke(now, newTokenHash);
        db.RefreshTokens.Add(RefreshTokenEntity.Create(user.Id, newTokenHash, newExpiresAt));

        var permissions = user.GetPermissionNames().ToList();
        var accessToken = jwtTokenService.GenerateAccessToken(user, permissions);

        await db.SaveChangesAsync(cancellationToken);

        return Result<AuthTokensDto>.Success(new AuthTokensDto
        {
            AccessToken = accessToken,
            AccessTokenExpiresAt = now.Add(jwtTokenService.AccessTokenLifetime),
            RefreshToken = newRawToken,
            RefreshTokenExpiresAt = newExpiresAt,
        });
    }
}
