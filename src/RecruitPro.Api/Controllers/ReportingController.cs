using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Reporting.Dtos;
using RecruitPro.Application.Reporting.GetDashboardSummary;

namespace RecruitPro.Api.Controllers;

[Route("api/v1/reporting")]
public sealed class ReportingController(ISender mediator) : ApiControllerBase
{
    [HttpGet("dashboard")]
    [RequirePermission("Reporting.Dashboard.View")]
    public async Task<ActionResult<ApiResponse<DashboardSummaryDto>>> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetDashboardSummaryQuery(), cancellationToken);
        return HandleResult(result);
    }
}
