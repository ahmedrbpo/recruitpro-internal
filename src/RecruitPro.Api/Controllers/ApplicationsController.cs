using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Applications.CreateApplication;
using RecruitPro.Application.Recruitment.Applications.GetSubmissionDetails;
using RecruitPro.Application.Recruitment.Applications.MoveApplicationStage;
using RecruitPro.Application.Recruitment.Applications.SetSubmissionDetails;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Application.Recruitment.Interviews.GetInterviewsForApplication;
using RecruitPro.Application.Recruitment.Offers.GetOfferByApplicationId;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Api.Controllers;

public sealed record CreateApplicationRequest(Guid JobId, Guid CandidateId);

public sealed record MoveApplicationStageRequest(string NewStage);

public sealed record SetSubmissionDetailsRequest(
    ApplicationWorkType WorkType,
    ApplicationInterviewType InterviewType,
    decimal CurrentCTC,
    decimal ExpectedCTC,
    string? UANNumber);

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

    [HttpPut("{id:guid}/submission-details")]
    [RequirePermission("Recruitment.Application.Create")]
    public async Task<ActionResult<ApiResponse<ApplicationDto>>> SetSubmissionDetails(
        Guid id, SetSubmissionDetailsRequest request, CancellationToken cancellationToken)
    {
        var command = new SetSubmissionDetailsCommand(id, request.WorkType, request.InterviewType,
            request.CurrentCTC, request.ExpectedCTC, request.UANNumber);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}/submission-details")]
    [RequirePermission("Recruitment.Application.View")]
    public async Task<ActionResult<ApiResponse<ApplicationSubmissionDetailsDto>>> GetSubmissionDetails(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetApplicationSubmissionDetailsQuery(id), cancellationToken);
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
