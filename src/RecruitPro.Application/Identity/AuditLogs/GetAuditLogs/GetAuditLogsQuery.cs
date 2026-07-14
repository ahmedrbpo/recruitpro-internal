using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.AuditLogs.GetAuditLogs;

public sealed record GetAuditLogsQuery(int Page = 1, int PageSize = 20, string? EntityType = null, Guid? EntityId = null)
    : IRequest<Result<PaginatedList<AuditLogDto>>>;
