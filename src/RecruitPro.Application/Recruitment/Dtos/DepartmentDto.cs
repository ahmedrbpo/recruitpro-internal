using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record DepartmentDto(
    Guid Id,
    string Name,
    string? DepartmentCode,
    string? Description,
    Guid? ParentDepartmentId,
    Guid? ManagerId,
    bool IsActive)
{
    public static DepartmentDto FromEntity(Department department) =>
        new(
            department.Id,
            department.Name,
            department.DepartmentCode,
            department.Description,
            department.ParentDepartmentId,
            department.ManagerId,
            department.IsActive);
}
