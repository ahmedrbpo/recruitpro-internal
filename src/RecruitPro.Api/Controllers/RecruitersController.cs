using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Application.Recruitment.Recruiters.CreateRecruiter;
using RecruitPro.Application.Recruitment.Recruiters.GetRecruiterById;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Api.Controllers;

public sealed record CreateRecruiterRequest(Guid UserId, RecruiterType Type, string? VendorCompany);

[Route("api/v1/recruiters")]
public sealed class RecruitersController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Recruitment.Recruiter.Create")]
    public async Task<ActionResult<ApiResponse<RecruiterDto>>> Create(CreateRecruiterRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateRecruiterCommand(request.UserId, request.Type, request.VendorCompany), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("Recruitment.Recruiter.View")]
    public async Task<ActionResult<ApiResponse<RecruiterDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRecruiterByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }
}
