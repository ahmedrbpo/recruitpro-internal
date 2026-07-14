using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Identity.Dtos;

public sealed record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string? LastName,
    string? Phone,
    bool IsActive,
    bool EmailVerified,
    DateTimeOffset? LastLoginAt,
    Guid? DepartmentId,
    IReadOnlyCollection<RoleSummaryDto> Roles)
{
    public static UserDto FromEntity(ApplicationUser user) =>
        new(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Phone,
            user.IsActive,
            user.EmailVerified,
            user.LastLoginAt,
            user.DepartmentId,
            user.UserRoles.Where(ur => ur.Role is not null).Select(ur => RoleSummaryDto.FromEntity(ur.Role!)).ToList());
}
