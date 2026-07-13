using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.Dtos;
using RecruitPro.Domain.Notifications.Entities;

namespace RecruitPro.Application.Notifications.Templates.CreateNotificationTemplate;

public sealed class CreateNotificationTemplateCommandHandler(IApplicationDbContext db)
    : IRequestHandler<CreateNotificationTemplateCommand, Result<NotificationTemplateDto>>
{
    public async Task<Result<NotificationTemplateDto>> Handle(CreateNotificationTemplateCommand request, CancellationToken cancellationToken)
    {
        var normalizedCode = request.Code.Trim().ToUpperInvariant();
        var codeTaken = await db.NotificationTemplates.AnyAsync(t => t.Code == normalizedCode, cancellationToken);
        if (codeTaken)
            return Result<NotificationTemplateDto>.Conflict($"A notification template with code '{normalizedCode}' already exists.");

        var template = NotificationTemplate.Create(request.Code, request.Name, request.Channel, request.Subject, request.Body);

        db.NotificationTemplates.Add(template);
        await db.SaveChangesAsync(cancellationToken);

        return Result<NotificationTemplateDto>.Success(NotificationTemplateDto.FromEntity(template));
    }
}
