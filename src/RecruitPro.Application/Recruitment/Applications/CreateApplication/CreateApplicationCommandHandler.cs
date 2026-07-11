using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Applications.CreateApplication;

public sealed class CreateApplicationCommandHandler(IApplicationDbContext db) : IRequestHandler<CreateApplicationCommand, Result<ApplicationDto>>
{
    public async Task<Result<ApplicationDto>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var jobExists = await db.Jobs.AnyAsync(j => j.Id == request.JobId, cancellationToken);
        if (!jobExists)
            return Result<ApplicationDto>.NotFound("Job not found.");

        var candidateExists = await db.Candidates.AnyAsync(c => c.Id == request.CandidateId, cancellationToken);
        if (!candidateExists)
            return Result<ApplicationDto>.NotFound("Candidate not found.");

        var alreadyApplied = await db.Applications.AnyAsync(
            a => a.JobId == request.JobId && a.CandidateId == request.CandidateId, cancellationToken);
        if (alreadyApplied)
            return Result<ApplicationDto>.Conflict("This candidate has already applied to this job.");

        var application = JobApplication.Create(request.JobId, request.CandidateId);
        db.Applications.Add(application);
        await db.SaveChangesAsync(cancellationToken);

        return Result<ApplicationDto>.Success(ApplicationDto.FromEntity(application));
    }
}
