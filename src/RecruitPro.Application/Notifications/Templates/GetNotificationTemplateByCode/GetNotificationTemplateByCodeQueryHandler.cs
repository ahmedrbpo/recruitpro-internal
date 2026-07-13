using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.Dtos;

namespace RecruitPro.Application.Notifications.Templates.GetNotificationTemplateByCode;

public sealed class GetNotificationTemplateByCodeQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetNotificationTemplateByCodeQuery, Result<NotificationTemplateDto>>
{
    public async Task<Result<NotificationTemplateDto>> Handle(GetNotificationTemplateByCodeQuery request, CancellationToken cancellationToken)
    {
        var normalizedCode = request.Code.Trim().ToUpperInvariant();
        var template = await db.NotificationTemplates.AsNoTracking()
            .SingleOrDefaultAsync(t => t.Code == normalizedCode, cancellationToken);

        return template is null
            ? Result<NotificationTemplateDto>.NotFound()
            : Result<NotificationTemplateDto>.Success(NotificationTemplateDto.FromEntity(template));
    }
}
