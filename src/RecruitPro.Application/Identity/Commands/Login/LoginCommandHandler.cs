using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;
using RecruitPro.Domain.Common.Exceptions;
// Aliased: "RecruitPro.Application.Identity.Commands.RefreshToken" is a sibling namespace under
// the same "Commands" parent, which shadows the unqualified "RefreshToken" entity type name.
using RefreshTokenEntity = RecruitPro.Domain.Identity.Entities.RefreshToken;

namespace RecruitPro.Application.Identity.Commands.Login;

public sealed class LoginCommandHandler(
    IApplicationDbContext db,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<LoginCommand, Result<AuthTokensDto>>
{
    public async Task<Result<AuthTokensDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();

        var user = await db.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r!.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);

        // Same error message whether the account doesn't exist, is inactive, or the password is
        // wrong — never let a login failure leak which case it was.
        const string invalidCredentialsMessage = "Invalid email or password.";

        if (user is null || !user.IsActive)
            return Result<AuthTokensDto>.Unauthorized(invalidCredentialsMessage);

        var now = dateTimeProvider.UtcNow;

        if (user.IsLockedOut(now))
            return Result<AuthTokensDto>.Unauthorized("Account is temporarily locked due to repeated failed sign-in attempts.");

        if (!passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            try
            {
                user.RegisterFailedLoginAttempt(now);
            }
            catch (AccountLockedOutException)
            {
                // A concurrent request already locked the account between our check and here.
            }

            await db.SaveChangesAsync(cancellationToken);
            return Result<AuthTokensDto>.Unauthorized(invalidCredentialsMessage);
        }

        user.RegisterSuccessfulLogin(now);

        var permissions = user.GetPermissionNames().ToList();
        var accessToken = jwtTokenService.GenerateAccessToken(user, permissions);
        var (rawRefreshToken, refreshTokenHash) = jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpiresAt = now.Add(jwtTokenService.RefreshTokenLifetime);

        db.RefreshTokens.Add(RefreshTokenEntity.Create(user.Id, refreshTokenHash, refreshTokenExpiresAt));

        await db.SaveChangesAsync(cancellationToken);

        return Result<AuthTokensDto>.Success(new AuthTokensDto
        {
            AccessToken = accessToken,
            AccessTokenExpiresAt = now.Add(jwtTokenService.AccessTokenLifetime),
            RefreshToken = rawRefreshToken,
            RefreshTokenExpiresAt = refreshTokenExpiresAt,
        });
    }
}
