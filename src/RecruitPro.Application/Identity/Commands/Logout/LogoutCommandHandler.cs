using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;

namespace RecruitPro.Application.Identity.Commands.Logout;

public sealed class LogoutCommandHandler(IApplicationDbContext db, IJwtTokenService jwtTokenService, IDateTimeProvider dateTimeProvider)
    : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = jwtTokenService.HashRefreshToken(request.RawRefreshToken);

        var token = await db.RefreshTokens.SingleOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

        // Logout is idempotent: an unknown or already-revoked token is not an error.
        if (token is not null && !token.IsRevoked)
        {
            token.Revoke(dateTimeProvider.UtcNow);
            await db.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}
