using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Departments.CreateDepartment;
using RecruitPro.Application.Recruitment.Departments.GetDepartmentById;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Api.Controllers;

public sealed record CreateDepartmentRequest(string Name, string? DepartmentCode, string? Description, Guid? ParentDepartmentId, Guid? ManagerId);

[Route("api/v1/departments")]
public sealed class DepartmentsController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Recruitment.Department.Create")]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> Create(CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentCommand(request.Name, request.DepartmentCode, request.Description, request.ParentDepartmentId, request.ManagerId);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("Recruitment.Department.View")]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetDepartmentByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }
}
