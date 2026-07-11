namespace RecruitPro.Application.Common.Interfaces;

/// <summary>Issues presigned URLs against Supabase Storage — files never pass through the API
/// process. Upload URLs are single-use and short-lived; download URLs expire in 5 minutes
/// per the blueprint's read flow.</summary>
public interface IFileStorageService
{
    Task<string> CreateUploadUrlAsync(string objectKey, CancellationToken cancellationToken);

    Task<string> CreateDownloadUrlAsync(string objectKey, TimeSpan expiry, CancellationToken cancellationToken);
}
