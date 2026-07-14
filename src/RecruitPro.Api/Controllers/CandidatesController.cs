using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Candidates.CreateCandidate;
using RecruitPro.Application.Recruitment.Candidates.GetCandidateProfile;
using RecruitPro.Application.Recruitment.Candidates.ManageEducation;
using RecruitPro.Application.Recruitment.Candidates.ManageEmployment;
using RecruitPro.Application.Recruitment.Candidates.UpdateCandidate;
using RecruitPro.Application.Recruitment.Candidates.UploadResume;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Api.Controllers;

public sealed record CreateCandidateRequest(string FirstName, string LastName, string Email, string? Phone);

public sealed record UpdateCandidateRequest(
    Gender? Gender,
    DateOnly? DateOfBirth,
    string? Pan,
    decimal? TotalExperienceYears,
    decimal? RelevantExperienceYears,
    string? StreetAddress,
    string? City,
    string? State,
    string? PostalCode,
    string? WorkLocation);

public sealed record AddCandidateEducationRequest(
    string College,
    string Degree,
    string? Stream,
    EducationType Type,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Location);

public sealed record AddCandidateEmploymentRequest(
    string PayrollCompany,
    string Company,
    string Designation,
    EmploymentHistoryType Type,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Location);

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

    [HttpPut("{id:guid}")]
    [RequirePermission("Recruitment.Candidate.Update")]
    public async Task<ActionResult<ApiResponse<CandidateDto>>> Update(Guid id, UpdateCandidateRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCandidateCommand(id, request.Gender, request.DateOfBirth, request.Pan,
            request.TotalExperienceYears, request.RelevantExperienceYears,
            request.StreetAddress, request.City, request.State, request.PostalCode, request.WorkLocation);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{candidateId:guid}/education")]
    [RequirePermission("Recruitment.Candidate.Update")]
    public async Task<ActionResult<ApiResponse<CandidateEducationDto>>> AddEducation(
        Guid candidateId, AddCandidateEducationRequest request, CancellationToken cancellationToken)
    {
        var command = new AddCandidateEducationCommand(candidateId, request.College, request.Degree,
            request.Stream, request.Type, request.StartDate, request.EndDate, request.Location);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{candidateId:guid}/education/{educationId:guid}")]
    [RequirePermission("Recruitment.Candidate.Update")]
    public async Task<ActionResult<ApiResponse<object>>> RemoveEducation(
        Guid candidateId, Guid educationId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveCandidateEducationCommand(candidateId, educationId), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{candidateId:guid}/employment")]
    [RequirePermission("Recruitment.Candidate.Update")]
    public async Task<ActionResult<ApiResponse<CandidateEmploymentHistoryDto>>> AddEmployment(
        Guid candidateId, AddCandidateEmploymentRequest request, CancellationToken cancellationToken)
    {
        var command = new AddCandidateEmploymentCommand(candidateId, request.PayrollCompany, request.Company,
            request.Designation, request.Type, request.StartDate, request.EndDate, request.Location);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{candidateId:guid}/employment/{employmentId:guid}")]
    [RequirePermission("Recruitment.Candidate.Update")]
    public async Task<ActionResult<ApiResponse<object>>> RemoveEmployment(
        Guid candidateId, Guid employmentId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveCandidateEmploymentCommand(candidateId, employmentId), cancellationToken);
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
