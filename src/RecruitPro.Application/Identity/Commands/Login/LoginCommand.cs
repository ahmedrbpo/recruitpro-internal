using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthTokensDto>>;
