using RecruitPro.Domain.Notifications.Entities;

namespace RecruitPro.Application.Notifications.Dtos;

public sealed record NotificationTemplateDto(
    Guid Id,
    string Code,
    string Name,
    NotificationChannel Channel,
    string Subject,
    string Body,
    bool IsActive)
{
    public static NotificationTemplateDto FromEntity(NotificationTemplate template) =>
        new(template.Id, template.Code, template.Name, template.Channel, template.Subject, template.Body, template.IsActive);
}
