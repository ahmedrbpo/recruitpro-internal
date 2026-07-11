using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record ApplicationStageHistoryDto(string FromStage, string ToStage, DateTimeOffset ChangedAt);

public sealed record ApplicationDto(
    Guid Id,
    Guid JobId,
    Guid CandidateId,
    string Stage,
    IReadOnlyCollection<ApplicationStageHistoryDto> StageHistory)
{
    public static ApplicationDto FromEntity(JobApplication application) =>
        new(
            application.Id,
            application.JobId,
            application.CandidateId,
            application.Stage,
            application.StageHistory
                .OrderBy(h => h.ChangedAt)
                .Select(h => new ApplicationStageHistoryDto(h.FromStage, h.ToStage, h.ChangedAt))
                .ToList());
}
