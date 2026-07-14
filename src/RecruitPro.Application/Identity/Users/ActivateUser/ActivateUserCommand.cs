using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Users.ActivateUser;

public sealed record ActivateUserCommand(Guid UserId) : IRequest<Result<UserDto>>;
