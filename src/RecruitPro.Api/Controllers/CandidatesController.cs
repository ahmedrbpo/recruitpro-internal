using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Candidates.CreateCandidate;
using RecruitPro.Application.Recruitment.Candidates.GetCandidateProfile;
using RecruitPro.Application.Recruitment.Candidates.UploadResume;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Api.Controllers;

public sealed record CreateCandidateRequest(string FirstName, string LastName, string Email, string? Phone);

public sealed record RequestResumeUploadRequest(string OriginalFileName, string ContentType, long SizeBytes);

[Route("api/v1/candidates")]
public sealed class CandidatesController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Recruitment.Candidate.Create")]
    public async Task<ActionResult<ApiResponse<CandidateDto>>> Create(CreateCandidateRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreateCandidateCommand(request.FirstName, request.LastName, request.Email, request.Phone), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("Recruitment.Candidate.View")]
    public async Task<ActionResult<ApiResponse<CandidateDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCandidateProfileQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{candidateId:guid}/resumes/upload-url")]
    [RequirePermission("Recruitment.Resume.Upload")]
    public async Task<ActionResult<ApiResponse<ResumeUploadDto>>> RequestResumeUploadUrl(
        Guid candidateId, RequestResumeUploadRequest request, CancellationToken cancellationToken)
    {
        var command = new RequestResumeUploadCommand(candidateId, request.OriginalFileName, request.ContentType, request.SizeBytes);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{candidateId:guid}/resumes/{resumeId:guid}/confirm")]
    [RequirePermission("Recruitment.Resume.Upload")]
    public async Task<ActionResult<ApiResponse<ResumeDto>>> ConfirmResumeUpload(
        Guid candidateId, Guid resumeId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ConfirmResumeUploadCommand(resumeId), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{candidateId:guid}/resumes/{resumeId:guid}/download-url")]
    [RequirePermission("Recruitment.Resume.Download")]
    public async Task<ActionResult<ApiResponse<ResumeDownloadUrlDto>>> GetResumeDownloadUrl(
        Guid candidateId, Guid resumeId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetResumeDownloadUrlQuery(resumeId), cancellationToken);
        return HandleResult(result);
    }
}
