using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Identity.Dtos;

public sealed record RoleSummaryDto(Guid Id, string Name, string Code)
{
    public static RoleSummaryDto FromEntity(Role role) => new(role.Id, role.Name, role.Code);
}
