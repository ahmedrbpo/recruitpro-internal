using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Permissions.GetPermissions;

public sealed record GetPermissionsQuery : IRequest<Result<IReadOnlyCollection<PermissionDto>>>;
