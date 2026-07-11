using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Candidates.UploadResume;

public sealed class GetResumeDownloadUrlQueryHandler(IApplicationDbContext db, IFileStorageService fileStorage, IDateTimeProvider dateTimeProvider)
    : IRequestHandler<GetResumeDownloadUrlQuery, Result<ResumeDownloadUrlDto>>
{
    // Per the blueprint's read flow: presigned GET URLs, 5-minute expiry, generated per request.
    private static readonly TimeSpan Expiry = TimeSpan.FromMinutes(5);

    public async Task<Result<ResumeDownloadUrlDto>> Handle(GetResumeDownloadUrlQuery request, CancellationToken cancellationToken)
    {
        var resume = await db.Resumes.AsNoTracking().SingleOrDefaultAsync(r => r.Id == request.ResumeId, cancellationToken);
        if (resume is null || !resume.IsConfirmed)
            return Result<ResumeDownloadUrlDto>.NotFound();

        var url = await fileStorage.CreateDownloadUrlAsync(resume.ObjectKey, Expiry, cancellationToken);
        var now = dateTimeProvider.UtcNow;

        return Result<ResumeDownloadUrlDto>.Success(new ResumeDownloadUrlDto(url, now.Add(Expiry)));
    }
}
