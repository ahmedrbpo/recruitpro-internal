using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Skills.GetSkillById;

public sealed class GetSkillByIdQueryHandler(IApplicationDbContext db) : IRequestHandler<GetSkillByIdQuery, Result<SkillDto>>
{
    public async Task<Result<SkillDto>> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
    {
        var skill = await db.Skills.AsNoTracking().SingleOrDefaultAsync(s => s.Id == request.SkillId, cancellationToken);

        return skill is null ? Result<SkillDto>.NotFound() : Result<SkillDto>.Success(SkillDto.FromEntity(skill));
    }
}
