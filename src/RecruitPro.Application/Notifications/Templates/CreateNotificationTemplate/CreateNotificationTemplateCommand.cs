using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.Dtos;
using RecruitPro.Domain.Notifications.Entities;

namespace RecruitPro.Application.Notifications.Templates.CreateNotificationTemplate;

public sealed record CreateNotificationTemplateCommand(
    string Code,
    string Name,
    NotificationChannel Channel,
    string Subject,
    string Body) : IRequest<Result<NotificationTemplateDto>>;
