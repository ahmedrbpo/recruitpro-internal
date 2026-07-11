using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Common;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Commands.Login;
using RecruitPro.Application.Identity.Commands.Logout;
using RecruitPro.Application.Identity.Commands.RefreshToken;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Api.Controllers;

public sealed record LoginRequest(string Email, string Password);

[Route("api/v1/auth")]
[AllowAnonymous]
public sealed class AuthController(ISender mediator) : ApiControllerBase
{
    private const string RefreshTokenCookieName = "refreshToken";

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthTokensDto>>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new LoginCommand(request.Email, request.Password), cancellationToken);

        if (result.IsSuccess)
            SetRefreshTokenCookie(result.Value!.RefreshToken, result.Value.RefreshTokenExpiresAt);

        return HandleResult(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<AuthTokensDto>>> Refresh(CancellationToken cancellationToken)
    {
        var rawToken = Request.Cookies[RefreshTokenCookieName];
        if (string.IsNullOrEmpty(rawToken))
            return Unauthorized(ApiResponse<AuthTokensDto>.Fail(new ApiError("Unauthorized", "Missing refresh token.")));

        var result = await mediator.Send(new RefreshTokenCommand(rawToken), cancellationToken);

        if (result.IsSuccess)
            SetRefreshTokenCookie(result.Value!.RefreshToken, result.Value.RefreshTokenExpiresAt);
        else
            Response.Cookies.Delete(RefreshTokenCookieName);

        return HandleResult(result);
    }

    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<object>>> Logout(CancellationToken cancellationToken)
    {
        var rawToken = Request.Cookies[RefreshTokenCookieName];
        if (!string.IsNullOrEmpty(rawToken))
            await mediator.Send(new LogoutCommand(rawToken), cancellationToken);

        Response.Cookies.Delete(RefreshTokenCookieName);
        return HandleResult(Result.Success());
    }

    private void SetRefreshTokenCookie(string rawToken, DateTimeOffset expiresAt) =>
        Response.Cookies.Append(RefreshTokenCookieName, rawToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expiresAt,
        });
}
