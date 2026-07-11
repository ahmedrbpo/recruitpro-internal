using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Applications.GetPipeline;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Application.Recruitment.Jobs.CreateJob;
using RecruitPro.Application.Recruitment.Jobs.GetJobById;
using RecruitPro.Application.Recruitment.Jobs.GetJobsPaginated;
using RecruitPro.Application.Recruitment.Jobs.PublishJob;

namespace RecruitPro.Api.Controllers;

public sealed record CreateJobRequest(
    string Title,
    Guid? DepartmentId,
    decimal? SalaryMin,
    decimal? SalaryMax,
    IReadOnlyCollection<string>? Skills);

[Route("api/v1/jobs")]
public sealed class JobsController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Recruitment.Job.Create")]
    public async Task<ActionResult<ApiResponse<JobDto>>> Create(CreateJobRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateJobCommand(request.Title, request.DepartmentId, request.SalaryMin, request.SalaryMax, request.Skills);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet]
    [RequirePermission("Recruitment.Job.View")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<JobDto>>>> GetPaginated(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetJobsPaginatedQuery(page, pageSize, status), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("Recruitment.Job.View")]
    public async Task<ActionResult<ApiResponse<JobDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetJobByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/publish")]
    [RequirePermission("Recruitment.Job.Publish")]
    public async Task<ActionResult<ApiResponse<JobDto>>> Publish(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new PublishJobCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}/pipeline")]
    [RequirePermission("Recruitment.Application.View")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<ApplicationDto>>>> GetPipeline(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPipelineQuery(id), cancellationToken);
        return HandleResult(result);
    }
}
