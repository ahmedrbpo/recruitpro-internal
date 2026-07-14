using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Identity.Permissions.CreatePermission;

public sealed record CreatePermissionCommand(string Name, string? Resource, PermissionAction? Action, string? Description)
    : IRequest<Result<PermissionDto>>;
