using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Candidates.UploadResume;

public sealed class RequestResumeUploadCommandHandler(IApplicationDbContext db, IFileStorageService fileStorage)
    : IRequestHandler<RequestResumeUploadCommand, Result<ResumeUploadDto>>
{
    public async Task<Result<ResumeUploadDto>> Handle(RequestResumeUploadCommand request, CancellationToken cancellationToken)
    {
        var candidateExists = await db.Candidates.AnyAsync(c => c.Id == request.CandidateId, cancellationToken);
        if (!candidateExists)
            return Result<ResumeUploadDto>.NotFound("Candidate not found.");

        var objectKey = $"resumes/{request.CandidateId}/{Guid.NewGuid()}{Path.GetExtension(request.OriginalFileName)}";

        // Throws UnsupportedResumeFileTypeException / ResumeFileTooLargeException if invalid —
        // left uncaught by design; ExceptionHandlingMiddleware maps DomainException to a 400 response.
        var resume = Resume.Create(request.CandidateId, objectKey, request.OriginalFileName, request.ContentType, request.SizeBytes);

        db.Resumes.Add(resume);
        await db.SaveChangesAsync(cancellationToken);

        var uploadUrl = await fileStorage.CreateUploadUrlAsync(objectKey, cancellationToken);

        return Result<ResumeUploadDto>.Success(new ResumeUploadDto(resume.Id, uploadUrl));
    }
}
