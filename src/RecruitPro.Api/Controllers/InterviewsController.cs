using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Application.Recruitment.Interviews.CancelInterview;
using RecruitPro.Application.Recruitment.Interviews.CompleteInterview;
using RecruitPro.Application.Recruitment.Interviews.RecordInterviewFeedback;
using RecruitPro.Application.Recruitment.Interviews.RescheduleInterview;
using RecruitPro.Application.Recruitment.Interviews.ScheduleInterview;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Api.Controllers;

public sealed record ScheduleInterviewRequest(
    Guid ApplicationId, DateTimeOffset ScheduledAt, InterviewMode Mode, int Round, Guid? InterviewerId, int? DurationMinutes, string? Notes);

public sealed record RescheduleInterviewRequest(DateTimeOffset NewScheduledAt);

public sealed record RecordInterviewFeedbackRequest(Guid InterviewerId, int Rating, RecommendationType Recommendation, string? Comments);

[Route("api/v1/interviews")]
public sealed class InterviewsController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Recruitment.Interview.Schedule")]
    public async Task<ActionResult<ApiResponse<InterviewDto>>> Schedule(ScheduleInterviewRequest request, CancellationToken cancellationToken)
    {
        var command = new ScheduleInterviewCommand(
            request.ApplicationId, request.ScheduledAt, request.Mode, request.Round, request.InterviewerId, request.DurationMinutes, request.Notes);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/reschedule")]
    [RequirePermission("Recruitment.Interview.Schedule")]
    public async Task<ActionResult<ApiResponse<InterviewDto>>> Reschedule(Guid id, RescheduleInterviewRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RescheduleInterviewCommand(id, request.NewScheduledAt), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/complete")]
    [RequirePermission("Recruitment.Interview.Schedule")]
    public async Task<ActionResult<ApiResponse<InterviewDto>>> Complete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CompleteInterviewCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/cancel")]
    [RequirePermission("Recruitment.Interview.Schedule")]
    public async Task<ActionResult<ApiResponse<InterviewDto>>> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CancelInterviewCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/feedback")]
    [RequirePermission("Recruitment.Interview.RecordFeedback")]
    public async Task<ActionResult<ApiResponse<InterviewDto>>> RecordFeedback(
        Guid id, RecordInterviewFeedbackRequest request, CancellationToken cancellationToken)
    {
        var command = new RecordInterviewFeedbackCommand(id, request.InterviewerId, request.Rating, request.Recommendation, request.Comments);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }
}
