namespace RecruitPro.Application.Reporting.Dtos;

/// <summary>Each count dictionary is sparse — a status/stage with zero matching rows is simply
/// absent from the map rather than present with a 0 value.</summary>
public sealed record DashboardSummaryDto(
    IReadOnlyDictionary<string, int> JobCountsByStatus,
    IReadOnlyDictionary<string, int> ApplicationCountsByStage,
    double? AverageTimeToHireDays,
    IReadOnlyDictionary<string, int> InterviewCountsByStatus,
    IReadOnlyDictionary<string, int> OfferCountsByStatus,
    int TotalCandidates);
