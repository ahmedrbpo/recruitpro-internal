using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Identity.Dtos;

public sealed record RoleDto(
    Guid Id,
    string Name,
    string Code,
    string? Description,
    bool IsSystem,
    bool IsActive,
    IReadOnlyCollection<PermissionSummaryDto> Permissions)
{
    public static RoleDto FromEntity(Role role) =>
        new(
            role.Id,
            role.Name,
            role.Code,
            role.Description,
            role.IsSystem,
            role.IsActive,
            role.RolePermissions.Where(rp => rp.Permission is not null).Select(rp => PermissionSummaryDto.FromEntity(rp.Permission!)).ToList());
}
