using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Roles.CreateRole;

public sealed record CreateRoleCommand(string Name, string Code, string? Description) : IRequest<Result<RoleDto>>;
