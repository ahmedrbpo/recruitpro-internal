using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Applications.GetSubmissionDetails;

public sealed class GetApplicationSubmissionDetailsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetApplicationSubmissionDetailsQuery, Result<ApplicationSubmissionDetailsDto>>
{
    public async Task<Result<ApplicationSubmissionDetailsDto>> Handle(GetApplicationSubmissionDetailsQuery request, CancellationToken cancellationToken)
    {
        // Job has no Client navigation property (only a Guid? ClientId, same as DepartmentId/
        // RecruiterId), so this needs an explicit join rather than .Include().
        var result = await (
            from a in db.Applications.AsNoTracking()
            join j in db.Jobs.AsNoTracking() on a.JobId equals j.Id
            join c in db.Clients.AsNoTracking() on j.ClientId equals c.Id into clientJoin
            from c in clientJoin.DefaultIfEmpty()
            where a.Id == request.ApplicationId
            select new ApplicationSubmissionDetailsDto(
                j.Title,
                c != null ? c.Name : null,
                j.JobCode,
                j.Onboarding,
                a.WorkType,
                a.InterviewType,
                a.CurrentCTC,
                a.ExpectedCTC,
                a.UANNumber)
        ).SingleOrDefaultAsync(cancellationToken);

        return result is null
            ? Result<ApplicationSubmissionDetailsDto>.NotFound()
            : Result<ApplicationSubmissionDetailsDto>.Success(result);
    }
}
