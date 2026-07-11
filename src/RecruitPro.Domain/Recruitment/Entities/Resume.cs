using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;

namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>
/// One row per uploaded resume file. The file itself lives in Supabase Storage
/// (bucket layout: resumes/{candidateId}/{fileId}.{ext}) — this entity only tracks metadata
/// and confirmation state; ObjectKey is the storage key, not the file content.
/// </summary>
public sealed class Resume : BaseEntity
{
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    };

    public const long MaxSizeBytes = 10 * 1024 * 1024; // 10 MB

    public Guid CandidateId { get; private set; }
    public Candidate? Candidate { get; private set; }
    public string ObjectKey { get; private set; } = default!;
    public string OriginalFileName { get; private set; } = default!;
    public string ContentType { get; private set; } = default!;
    public long SizeBytes { get; private set; }
    public bool IsConfirmed { get; private set; }
    public DateTimeOffset? ConfirmedAt { get; private set; }

    private Resume() { } // EF Core

    /// <summary>Enforces the file upload safety invariants: an accepted content type and a size cap.</summary>
    public static Resume Create(Guid candidateId, string objectKey, string originalFileName, string contentType, long sizeBytes)
    {
        if (!AllowedContentTypes.Contains(contentType))
            throw new UnsupportedResumeFileTypeException(contentType);
        if (sizeBytes > MaxSizeBytes)
            throw new ResumeFileTooLargeException(sizeBytes, MaxSizeBytes);
        if (sizeBytes <= 0)
            throw new ArgumentOutOfRangeException(nameof(sizeBytes), "File size must be greater than zero.");

        return new Resume
        {
            CandidateId = candidateId,
            ObjectKey = objectKey,
            OriginalFileName = originalFileName,
            ContentType = contentType,
            SizeBytes = sizeBytes,
        };
    }

    public void Confirm(DateTimeOffset now)
    {
        IsConfirmed = true;
        ConfirmedAt = now;
    }
}
