using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.Dtos;

namespace RecruitPro.Application.Notifications.Templates.GetNotificationTemplateByCode;

public sealed record GetNotificationTemplateByCodeQuery(string Code) : IRequest<Result<NotificationTemplateDto>>;
