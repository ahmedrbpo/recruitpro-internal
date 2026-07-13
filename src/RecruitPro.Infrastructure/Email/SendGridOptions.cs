namespace RecruitPro.Infrastructure.Email;

public sealed class SendGridOptions
{
    public const string SectionName = "SendGrid";

    public required string ApiKey { get; init; }
    public required string FromEmail { get; init; }
    public required string FromName { get; init; }
}
