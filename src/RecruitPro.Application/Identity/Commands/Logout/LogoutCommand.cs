using MediatR;
using RecruitPro.Application.Common.Models;

namespace RecruitPro.Application.Identity.Commands.Logout;

public sealed record LogoutCommand(string RawRefreshToken) : IRequest<Result>;
