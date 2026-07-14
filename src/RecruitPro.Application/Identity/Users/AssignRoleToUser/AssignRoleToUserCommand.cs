using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Users.AssignRoleToUser;

public sealed record AssignRoleToUserCommand(Guid UserId, Guid RoleId) : IRequest<Result<UserDto>>;
