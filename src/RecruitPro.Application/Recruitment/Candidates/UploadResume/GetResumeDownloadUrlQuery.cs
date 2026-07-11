using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Candidates.UploadResume;

public sealed record GetResumeDownloadUrlQuery(Guid ResumeId) : IRequest<Result<ResumeDownloadUrlDto>>;
