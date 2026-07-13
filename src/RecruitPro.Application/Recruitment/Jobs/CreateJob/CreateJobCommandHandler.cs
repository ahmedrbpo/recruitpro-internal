using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.ValueObjects;

namespace RecruitPro.Application.Recruitment.Jobs.CreateJob;

public sealed class CreateJobCommandHandler(IApplicationDbContext db) : IRequestHandler<CreateJobCommand, Result<JobDto>>
{
    public async Task<Result<JobDto>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = Job.Create(
            request.JobCode,
            request.Title,
            request.Description,
            request.EmploymentType,
            request.WorkMode,
            request.ExperienceMin,
            request.ExperienceMax,
            request.CurrencyCode,
            request.ClientId,
            request.JobCategoryId,
            request.DepartmentId,
            request.RecruiterId,
            request.Notes);

        if (request.SalaryMin.HasValue && request.SalaryMax.HasValue)
            job.SetSalaryRange(new SalaryRange(request.SalaryMin.Value, request.SalaryMax.Value));

        foreach (var skillName in request.Skills ?? [])
        {
            var skillId = await ResolveOrCreateSkillAsync(skillName, cancellationToken);
            job.AddSkill(skillId);
        }

        db.Jobs.Add(job);
        await db.SaveChangesAsync(cancellationToken);

        return Result<JobDto>.Success(JobDto.FromEntity(job));
    }

    /// <summary>Looks up an existing Skill by name (case-insensitive) or creates it — keeps the
    /// CreateJob API accepting plain skill names while the schema underneath is a proper
    /// Skill/JobSkill many-to-many. Fetches the full entity (not a scalar Id projection) so it
    /// ends up tracked in this context — that's what lets EF's automatic relationship fixup
    /// populate JobSkill.Skill on the in-memory graph before it's mapped to the response DTO;
    /// a scalar projection leaves the navigation null until the job is reloaded.</summary>
    private async Task<Guid> ResolveOrCreateSkillAsync(string skillName, CancellationToken cancellationToken)
    {
        var normalizedName = skillName.Trim();
        var normalizedNameLower = normalizedName.ToLowerInvariant();

        var existing = await db.Skills.FirstOrDefaultAsync(s => s.Name.ToLower() == normalizedNameLower, cancellationToken);
        if (existing is not null)
            return existing.Id;

        var skill = Skill.Create(normalizedName);
        db.Skills.Add(skill);
        await db.SaveChangesAsync(cancellationToken);

        return skill.Id;
    }
}
