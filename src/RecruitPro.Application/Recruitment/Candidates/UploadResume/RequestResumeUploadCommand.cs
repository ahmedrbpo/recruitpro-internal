using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Candidates.UploadResume;

public sealed record RequestResumeUploadCommand(Guid CandidateId, string OriginalFileName, string ContentType, long SizeBytes)
    : IRequest<Result<ResumeUploadDto>>;
