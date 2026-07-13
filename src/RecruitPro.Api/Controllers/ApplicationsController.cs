using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Applications.CreateApplication;
using RecruitPro.Application.Recruitment.Applications.MoveApplicationStage;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Application.Recruitment.Interviews.GetInterviewsForApplication;
using RecruitPro.Application.Recruitment.Offers.GetOfferByApplicationId;

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

    [HttpGet("{id:guid}/interviews")]
    [RequirePermission("Recruitment.Interview.View")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<InterviewDto>>>> GetInterviews(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetInterviewsForApplicationQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}/offer")]
    [RequirePermission("Recruitment.Offer.View")]
    public async Task<ActionResult<ApiResponse<OfferDto>>> GetOffer(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOfferByApplicationIdQuery(id), cancellationToken);
        return HandleResult(result);
    }
}
