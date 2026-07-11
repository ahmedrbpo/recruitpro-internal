using MediatR;
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
        var job = Job.Create(request.Title, request.DepartmentId);

        if (request.SalaryMin.HasValue && request.SalaryMax.HasValue)
            job.SetSalaryRange(new SalaryRange(request.SalaryMin.Value, request.SalaryMax.Value));

        foreach (var skill in request.Skills ?? [])
            job.AddSkill(skill);

        db.Jobs.Add(job);
        await db.SaveChangesAsync(cancellationToken);

        return Result<JobDto>.Success(JobDto.FromEntity(job));
    }
}
