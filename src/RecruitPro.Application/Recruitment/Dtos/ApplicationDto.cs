using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record ApplicationStageHistoryDto(string FromStage, string ToStage, DateTimeOffset ChangedAt);

public sealed record ApplicationDto(
    Guid Id,
    Guid JobId,
    Guid CandidateId,
    string Stage,
    ApplicationWorkType? WorkType,
    ApplicationInterviewType? InterviewType,
    decimal? CurrentCTC,
    decimal? ExpectedCTC,
    string? UANNumber,
    IReadOnlyCollection<ApplicationStageHistoryDto> StageHistory)
{
    public static ApplicationDto FromEntity(JobApplication application) =>
        new(
            application.Id,
            application.JobId,
            application.CandidateId,
            application.Stage,
            application.WorkType,
            application.InterviewType,
            application.CurrentCTC,
            application.ExpectedCTC,
            application.UANNumber,
            application.StageHistory
                .OrderBy(h => h.ChangedAt)
                .Select(h => new ApplicationStageHistoryDto(h.FromStage, h.ToStage, h.ChangedAt))
                .ToList());
}
