using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Roles.GetRoles;

public sealed record GetRolesQuery : IRequest<Result<IReadOnlyCollection<RoleDto>>>;
