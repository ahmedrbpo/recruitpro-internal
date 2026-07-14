using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Users.RevokeRoleFromUser;

public sealed record RevokeRoleFromUserCommand(Guid UserId, Guid RoleId) : IRequest<Result<UserDto>>;
