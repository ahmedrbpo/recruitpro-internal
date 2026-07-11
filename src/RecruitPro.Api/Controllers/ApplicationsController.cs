using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Applications.CreateApplication;
using RecruitPro.Application.Recruitment.Applications.MoveApplicationStage;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Api.Controllers;

public sealed record CreateApplicationRequest(Guid JobId, Guid CandidateId);

public sealed record MoveApplicationStageRequest(string NewStage);

[Route("api/v1/applications")]
public sealed class ApplicationsController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Recruitment.Application.Create")]
    public async Task<ActionResult<ApiResponse<ApplicationDto>>> Create(CreateApplicationRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateApplicationCommand(request.JobId, request.CandidateId), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/stage")]
    [RequirePermission("Recruitment.Application.MoveStage")]
    public async Task<ActionResult<ApiResponse<ApplicationDto>>> MoveStage(
        Guid id, MoveApplicationStageRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new MoveApplicationStageCommand(id, request.NewStage), cancellationToken);
        return HandleResult(result);
    }
}
