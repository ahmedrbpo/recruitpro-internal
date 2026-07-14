using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Roles.GetRoleById;

public sealed record GetRoleByIdQuery(Guid RoleId) : IRequest<Result<RoleDto>>;
