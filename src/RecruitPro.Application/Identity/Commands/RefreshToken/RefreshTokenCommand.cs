using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RawRefreshToken) : IRequest<Result<AuthTokensDto>>;
