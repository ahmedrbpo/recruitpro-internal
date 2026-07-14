using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Candidates.ManageEmployment;

public sealed class AddCandidateEmploymentCommandHandler(IApplicationDbContext db)
    : IRequestHandler<AddCandidateEmploymentCommand, Result<CandidateEmploymentHistoryDto>>
{
    public async Task<Result<CandidateEmploymentHistoryDto>> Handle(AddCandidateEmploymentCommand request, CancellationToken cancellationToken)
    {
        var candidateExists = await db.Candidates.AnyAsync(c => c.Id == request.CandidateId, cancellationToken);
        if (!candidateExists)
            return Result<CandidateEmploymentHistoryDto>.NotFound("Candidate not found.");

        // Throws InvalidDateRangeException if EndDate precedes StartDate — left uncaught by
        // design; ExceptionHandlingMiddleware maps DomainException to a 400 response.
        var employment = CandidateEmploymentHistory.Create(request.CandidateId, request.PayrollCompany,
            request.Company, request.Designation, request.Type, request.StartDate, request.EndDate, request.Location);

        db.CandidateEmploymentHistories.Add(employment);
        await db.SaveChangesAsync(cancellationToken);

        return Result<CandidateEmploymentHistoryDto>.Success(CandidateEmploymentHistoryDto.FromEntity(employment));
    }
}
