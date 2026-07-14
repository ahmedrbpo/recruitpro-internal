using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Identity.Dtos;

public sealed record PermissionSummaryDto(Guid Id, string Name)
{
    public static PermissionSummaryDto FromEntity(Permission permission) => new(permission.Id, permission.Name);
}
