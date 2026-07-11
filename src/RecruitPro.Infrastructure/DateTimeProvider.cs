using RecruitPro.Application.Common.Interfaces;

namespace RecruitPro.Infrastructure;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
