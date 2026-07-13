using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class Department : BaseEntity
{
    public Guid DepartmentExtId { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string? DepartmentCode { get; private set; }
    public string? Description { get; private set; }
    public Guid? ParentDepartmentId { get; private set; }
    public Guid? ManagerId { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Department() { } // EF Core

    public static Department Create(string name, string? departmentCode = null, string? description = null, Guid? parentDepartmentId = null, Guid? managerId = null) =>
        new()
        {
            Name = name,
            DepartmentCode = departmentCode,
            Description = description,
            ParentDepartmentId = parentDepartmentId,
            ManagerId = managerId,
        };
}
