using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record ResumeDto(
    Guid Id,
    string OriginalFileName,
    string ContentType,
    long SizeBytes,
    bool IsConfirmed)
{
    public static ResumeDto FromEntity(Resume resume) =>
        new(resume.Id, resume.OriginalFileName, resume.ContentType, resume.SizeBytes, resume.IsConfirmed);
}
