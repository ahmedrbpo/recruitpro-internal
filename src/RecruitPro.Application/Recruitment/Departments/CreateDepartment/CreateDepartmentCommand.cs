using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Departments.CreateDepartment;

public sealed record CreateDepartmentCommand(
    string Name,
    string? DepartmentCode,
    string? Description,
    Guid? ParentDepartmentId,
    Guid? ManagerId) : IRequest<Result<DepartmentDto>>;
