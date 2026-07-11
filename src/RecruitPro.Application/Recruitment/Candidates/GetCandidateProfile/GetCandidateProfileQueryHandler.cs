using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Candidates.GetCandidateProfile;

public sealed class GetCandidateProfileQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetCandidateProfileQuery, Result<CandidateDto>>
{
    public async Task<Result<CandidateDto>> Handle(GetCandidateProfileQuery request, CancellationToken cancellationToken)
    {
        var candidate = await db.Candidates.AsNoTracking().Include(c => c.Resumes)
            .SingleOrDefaultAsync(c => c.Id == request.CandidateId, cancellationToken);

        return candidate is null
            ? Result<CandidateDto>.NotFound()
            : Result<CandidateDto>.Success(CandidateDto.FromEntity(candidate));
    }
}
