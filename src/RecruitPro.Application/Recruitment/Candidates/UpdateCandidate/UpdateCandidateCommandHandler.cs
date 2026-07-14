using MediatR;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Candidates.UpdateCandidate;

public sealed class UpdateCandidateCommandHandler(IApplicationDbContext db)
    : IRequestHandler<UpdateCandidateCommand, Result<CandidateDto>>
{
    public async Task<Result<CandidateDto>> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await db.Candidates.FindAsync([request.CandidateId], cancellationToken);
        if (candidate is null)
            return Result<CandidateDto>.NotFound();

        candidate.UpdatePersonalDetails(request.Gender, request.DateOfBirth, request.Pan,
            request.TotalExperienceYears, request.RelevantExperienceYears,
            request.StreetAddress, request.City, request.State, request.PostalCode, request.WorkLocation);
        await db.SaveChangesAsync(cancellationToken);

        return Result<CandidateDto>.Success(CandidateDto.FromEntity(candidate));
    }
}
