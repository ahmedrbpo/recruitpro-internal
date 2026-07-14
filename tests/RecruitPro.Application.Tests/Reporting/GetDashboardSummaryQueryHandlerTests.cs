using FluentAssertions;
using RecruitPro.Application.Reporting.GetDashboardSummary;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.ValueObjects;
using Xunit;

namespace RecruitPro.Application.Tests.Reporting;

public sealed class GetDashboardSummaryQueryHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private GetDashboardSummaryQueryHandler CreateHandler() => new(_db);

    [Fact]
    public async Task Handle_NoData_ReturnsEmptyCountsAndNullTimeToHire()
    {
        var result = await CreateHandler().Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.JobCountsByStatus.Should().BeEmpty();
        result.Value.AverageTimeToHireDays.Should().BeNull();
        result.Value.TotalCandidates.Should().Be(0);
    }

    [Fact]
    public async Task Handle_JobsWithVariousStatuses_GroupsCountsByStatus()
    {
        _db.Jobs.Add(Job.Create("RP-1", "Job 1", "desc", EmploymentType.FullTime, WorkMode.Remote, 1, 3, "INR"));
        _db.Jobs.Add(Job.Create("RP-2", "Job 2", "desc", EmploymentType.FullTime, WorkMode.Remote, 1, 3, "INR"));
        var published = Job.Create("RP-3", "Job 3", "desc", EmploymentType.FullTime, WorkMode.Remote, 1, 3, "INR");
        published.SetSalaryRange(new SalaryRange(1_000_000, 2_000_000));
        published.Publish(DateOnly.FromDateTime(DateTime.UtcNow));
        _db.Jobs.Add(published);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

        result.Value!.JobCountsByStatus["Draft"].Should().Be(2);
        result.Value.JobCountsByStatus["Published"].Should().Be(1);
    }

    [Fact]
    public async Task Handle_HiredApplication_ComputesTimeToHireFromCreatedAtToHiredTransition()
    {
        var now = new DateTimeOffset(2026, 1, 20, 0, 0, 0, TimeSpan.Zero);
        var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid());
        application.CreatedAt = now.AddDays(-10);
        application.MoveToStage(ApplicationStage.Screening, now.AddDays(-8), null);
        application.MoveToStage(ApplicationStage.Interview, now.AddDays(-6), null);
        application.MoveToStage(ApplicationStage.Offer, now.AddDays(-4), null);
        application.MoveToStage(ApplicationStage.Hired, now.AddDays(-2), null);
        _db.Applications.Add(application);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

        result.Value!.AverageTimeToHireDays.Should().Be(8);
        result.Value.ApplicationCountsByStage[ApplicationStage.Hired].Should().Be(1);
    }

    [Fact]
    public async Task Handle_ApplicationNotYetHired_ExcludedFromTimeToHireAverage()
    {
        var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid());
        application.MoveToStage(ApplicationStage.Screening, DateTimeOffset.UtcNow, null);
        _db.Applications.Add(application);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

        result.Value!.AverageTimeToHireDays.Should().BeNull();
    }
}
