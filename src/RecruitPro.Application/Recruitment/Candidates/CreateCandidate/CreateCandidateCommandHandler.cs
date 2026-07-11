using MediatR;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Candidates.CreateCandidate;

public sealed class CreateCandidateCommandHandler(IApplicationDbContext db) : IRequestHandler<CreateCandidateCommand, Result<CandidateDto>>
{
    public async Task<Result<CandidateDto>> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = Candidate.Create(request.FirstName, request.LastName, request.Email, request.Phone);

        db.Candidates.Add(candidate);
        await db.SaveChangesAsync(cancellationToken);

        return Result<CandidateDto>.Success(CandidateDto.FromEntity(candidate));
    }
}
