using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Identity.Dtos;

public sealed record PermissionDto(
    Guid Id,
    string Name,
    string? Resource,
    PermissionAction? Action,
    string? Description,
    bool IsActive)
{
    public static PermissionDto FromEntity(Permission permission) =>
        new(permission.Id, permission.Name, permission.Resource, permission.Action, permission.Description, permission.IsActive);
}
