using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Skills.CreateSkill;

public sealed class CreateSkillCommandHandler(IApplicationDbContext db) : IRequestHandler<CreateSkillCommand, Result<SkillDto>>
{
    public async Task<Result<SkillDto>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        var nameTaken = await db.Skills.AnyAsync(s => s.Name == request.Name, cancellationToken);
        if (nameTaken)
            return Result<SkillDto>.Conflict($"Skill '{request.Name}' already exists.");

        var skill = Skill.Create(request.Name, request.Category, request.Description);

        db.Skills.Add(skill);
        await db.SaveChangesAsync(cancellationToken);

        return Result<SkillDto>.Success(SkillDto.FromEntity(skill));
    }
}
