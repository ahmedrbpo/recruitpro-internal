using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Departments.GetDepartmentById;

public sealed record GetDepartmentByIdQuery(Guid DepartmentId) : IRequest<Result<DepartmentDto>>;
