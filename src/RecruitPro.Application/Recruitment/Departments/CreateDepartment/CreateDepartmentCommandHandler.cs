using MediatR;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Departments.CreateDepartment;

public sealed class CreateDepartmentCommandHandler(IApplicationDbContext db) : IRequestHandler<CreateDepartmentCommand, Result<DepartmentDto>>
{
    public async Task<Result<DepartmentDto>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = Department.Create(
            request.Name, request.DepartmentCode, request.Description, request.ParentDepartmentId, request.ManagerId);

        db.Departments.Add(department);
        await db.SaveChangesAsync(cancellationToken);

        return Result<DepartmentDto>.Success(DepartmentDto.FromEntity(department));
    }
}
