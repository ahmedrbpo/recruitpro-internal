using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Recruiters.GetRecruiterById;

public sealed class GetRecruiterByIdQueryHandler(IApplicationDbContext db) : IRequestHandler<GetRecruiterByIdQuery, Result<RecruiterDto>>
{
    public async Task<Result<RecruiterDto>> Handle(GetRecruiterByIdQuery request, CancellationToken cancellationToken)
    {
        var recruiter = await db.Recruiters.AsNoTracking().SingleOrDefaultAsync(r => r.Id == request.RecruiterId, cancellationToken);

        return recruiter is null ? Result<RecruiterDto>.NotFound() : Result<RecruiterDto>.Success(RecruiterDto.FromEntity(recruiter));
    }
}
