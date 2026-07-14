using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Candidates.ManageEducation;

public sealed class AddCandidateEducationCommandHandler(IApplicationDbContext db)
    : IRequestHandler<AddCandidateEducationCommand, Result<CandidateEducationDto>>
{
    public async Task<Result<CandidateEducationDto>> Handle(AddCandidateEducationCommand request, CancellationToken cancellationToken)
    {
        var candidateExists = await db.Candidates.AnyAsync(c => c.Id == request.CandidateId, cancellationToken);
        if (!candidateExists)
            return Result<CandidateEducationDto>.NotFound("Candidate not found.");

        // Throws InvalidDateRangeException if EndDate precedes StartDate — left uncaught by
        // design; ExceptionHandlingMiddleware maps DomainException to a 400 response.
        var education = CandidateEducation.Create(request.CandidateId, request.College, request.Degree,
            request.Stream, request.Type, request.StartDate, request.EndDate, request.Location);

        db.CandidateEducations.Add(education);
        await db.SaveChangesAsync(cancellationToken);

        return Result<CandidateEducationDto>.Success(CandidateEducationDto.FromEntity(education));
    }
}
