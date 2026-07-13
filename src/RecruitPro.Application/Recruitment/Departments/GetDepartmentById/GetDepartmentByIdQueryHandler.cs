using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Departments.GetDepartmentById;

public sealed class GetDepartmentByIdQueryHandler(IApplicationDbContext db) : IRequestHandler<GetDepartmentByIdQuery, Result<DepartmentDto>>
{
    public async Task<Result<DepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await db.Departments.AsNoTracking().SingleOrDefaultAsync(d => d.Id == request.DepartmentId, cancellationToken);

        return department is null ? Result<DepartmentDto>.NotFound() : Result<DepartmentDto>.Success(DepartmentDto.FromEntity(department));
    }
}
