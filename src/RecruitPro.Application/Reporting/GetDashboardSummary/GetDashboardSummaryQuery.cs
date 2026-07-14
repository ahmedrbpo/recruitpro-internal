using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Reporting.Dtos;

namespace RecruitPro.Application.Reporting.GetDashboardSummary;

public sealed record GetDashboardSummaryQuery : IRequest<Result<DashboardSummaryDto>>;
