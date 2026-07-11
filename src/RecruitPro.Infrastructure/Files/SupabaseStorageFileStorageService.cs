using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using RecruitPro.Application.Common.Interfaces;

namespace RecruitPro.Infrastructure.Files;

/// <summary>
/// Calls Supabase Storage's native signed-URL endpoints directly (not the S3-compatibility mode,
/// which would require separate project configuration). The HttpClient is registered with
/// BaseAddress = SupabaseStorageOptions.Url and a service-role bearer token — see DependencyInjection.
/// </summary>
public sealed class SupabaseStorageFileStorageService(HttpClient httpClient, IOptions<SupabaseStorageOptions> options)
    : IFileStorageService
{
    private readonly SupabaseStorageOptions _options = options.Value;

    public async Task<string> CreateUploadUrlAsync(string objectKey, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsync(
            $"/storage/v1/object/upload/sign/{_options.Bucket}/{objectKey}", content: null, cancellationToken);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<SignedUploadUrlResponse>(cancellationToken: cancellationToken);
        return $"{_options.Url}/storage/v1{body!.Url}";
    }

    public async Task<string> CreateDownloadUrlAsync(string objectKey, TimeSpan expiry, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(
            $"/storage/v1/object/sign/{_options.Bucket}/{objectKey}",
            new { expiresIn = (int)expiry.TotalSeconds },
            cancellationToken);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<SignedDownloadUrlResponse>(cancellationToken: cancellationToken);
        return $"{_options.Url}/storage/v1{body!.SignedUrl}";
    }

    private sealed record SignedUploadUrlResponse([property: JsonPropertyName("url")] string Url);

    private sealed record SignedDownloadUrlResponse([property: JsonPropertyName("signedURL")] string SignedUrl);
}
