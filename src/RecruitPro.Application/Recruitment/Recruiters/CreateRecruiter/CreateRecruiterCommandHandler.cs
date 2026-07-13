using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Recruiters.CreateRecruiter;

public sealed class CreateRecruiterCommandHandler(IApplicationDbContext db) : IRequestHandler<CreateRecruiterCommand, Result<RecruiterDto>>
{
    public async Task<Result<RecruiterDto>> Handle(CreateRecruiterCommand request, CancellationToken cancellationToken)
    {
        var userExists = await db.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
        if (!userExists)
            return Result<RecruiterDto>.NotFound("User not found.");

        var recruiter = Recruiter.Create(request.UserId, request.Type, request.VendorCompany);

        db.Recruiters.Add(recruiter);
        await db.SaveChangesAsync(cancellationToken);

        return Result<RecruiterDto>.Success(RecruiterDto.FromEntity(recruiter));
    }
}
