using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using RecruitPro.Application.Common.Interfaces;

namespace RecruitPro.Infrastructure.Email;

/// <summary>Calls SendGrid's v3 REST API directly (not the official SDK) to stay consistent
/// with SupabaseStorageFileStorageService's typed-HttpClient pattern rather than adding a
/// vendor SDK dependency. BaseAddress and the Bearer API key are set once on the HttpClient in
/// DependencyInjection.</summary>
public sealed class SendGridEmailService(HttpClient httpClient, IOptions<SendGridOptions> options) : IEmailService
{
    private readonly SendGridOptions _options = options.Value;

    public async Task SendAsync(string toEmail, string? toName, string subject, string htmlBody, CancellationToken cancellationToken)
    {
        var payload = new
        {
            personalizations = new[]
            {
                new { to = new[] { new { email = toEmail, name = toName } } },
            },
            from = new { email = _options.FromEmail, name = _options.FromName },
            subject,
            content = new[] { new { type = "text/html", value = htmlBody } },
        };

        var response = await httpClient.PostAsJsonAsync("/v3/mail/send", payload, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
