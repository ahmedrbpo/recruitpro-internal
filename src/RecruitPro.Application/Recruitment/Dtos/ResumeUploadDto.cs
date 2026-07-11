namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record ResumeUploadDto(Guid ResumeId, string UploadUrl);

public sealed record ResumeDownloadUrlDto(string Url, DateTimeOffset ExpiresAt);
