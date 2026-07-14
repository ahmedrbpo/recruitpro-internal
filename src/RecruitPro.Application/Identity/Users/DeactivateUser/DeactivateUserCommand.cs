using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Users.DeactivateUser;

public sealed record DeactivateUserCommand(Guid UserId) : IRequest<Result<UserDto>>;
