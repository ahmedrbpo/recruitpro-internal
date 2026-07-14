using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Reporting.Dtos;
using RecruitPro.Domain.Recruitment.ValueObjects;

namespace RecruitPro.Application.Reporting.GetDashboardSummary;

public sealed class GetDashboardSummaryQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetDashboardSummaryQuery, Result<DashboardSummaryDto>>
{
    public async Task<Result<DashboardSummaryDto>> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        var jobCounts = await db.Jobs.AsNoTracking()
            .GroupBy(j => j.Status)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var applicationCountsByStage = await db.Applications.AsNoTracking()
            .GroupBy(a => a.Stage)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count, cancellationToken);

        var interviewCounts = await db.Interviews.AsNoTracking()
            .GroupBy(i => i.Status)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var offerCounts = await db.Offers.AsNoTracking()
            .GroupBy(o => o.Status)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var totalCandidates = await db.Candidates.CountAsync(cancellationToken);

        var averageTimeToHireDays = await ComputeAverageTimeToHireDaysAsync(db, cancellationToken);

        // Enum keys are converted to their string names in memory, after materialization —
        // deliberately not inside the LINQ query (g.Key.ToString() in a Select), to avoid
        // depending on the Npgsql provider's translation of enum-to-string within a GroupBy
        // projection, which isn't something worth gambling on for a handful of summary rows.
        var summary = new DashboardSummaryDto(
            jobCounts.ToDictionary(x => x.Key.ToString(), x => x.Count),
            applicationCountsByStage,
            averageTimeToHireDays,
            interviewCounts.ToDictionary(x => x.Key.ToString(), x => x.Count),
            offerCounts.ToDictionary(x => x.Key.ToString(), x => x.Count),
            totalCandidates);

        return Result<DashboardSummaryDto>.Success(summary);
    }

    /// <summary>Time-to-hire per application = the ChangedAt of its "moved to Hired" stage-history
    /// row minus the application's CreatedAt (the moment it entered "applied"). Computed
    /// in-memory over the (small, bounded-by-hire-volume) set of hired applications rather than
    /// as a single SQL aggregate — clearer than the date-arithmetic subquery this would otherwise
    /// require, and this query isn't hot-path.</summary>
    private static async Task<double?> ComputeAverageTimeToHireDaysAsync(IApplicationDbContext db, CancellationToken cancellationToken)
    {
        var hiredApplications = await db.Applications.AsNoTracking()
            .Where(a => a.Stage == ApplicationStage.Hired)
            .Include(a => a.StageHistory)
            .ToListAsync(cancellationToken);

        var timeToHireDays = new List<double>();
        foreach (var application in hiredApplications)
        {
            var hiredEntry = application.StageHistory.SingleOrDefault(h => h.ToStage == ApplicationStage.Hired);
            if (hiredEntry is not null)
                timeToHireDays.Add((hiredEntry.ChangedAt - application.CreatedAt).TotalDays);
        }

        return timeToHireDays.Count > 0 ? timeToHireDays.Average() : null;
    }
}
