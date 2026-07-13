using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Interviews.GetInterviewsForApplication;

public sealed class GetInterviewsForApplicationQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetInterviewsForApplicationQuery, Result<IReadOnlyCollection<InterviewDto>>>
{
    public async Task<Result<IReadOnlyCollection<InterviewDto>>> Handle(GetInterviewsForApplicationQuery request, CancellationToken cancellationToken)
    {
        var interviews = await db.Interviews.AsNoTracking()
            .Include(i => i.Feedback)
            .Where(i => i.ApplicationId == request.ApplicationId)
            .OrderBy(i => i.Round)
            .ToListAsync(cancellationToken);

        IReadOnlyCollection<InterviewDto> dtos = interviews.Select(InterviewDto.FromEntity).ToList();

        return Result<IReadOnlyCollection<InterviewDto>>.Success(dtos);
    }
}
