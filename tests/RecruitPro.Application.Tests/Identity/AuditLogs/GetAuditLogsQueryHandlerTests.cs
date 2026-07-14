using FluentAssertions;
using RecruitPro.Application.Identity.AuditLogs.GetAuditLogs;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Identity.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Identity.AuditLogs;

public sealed class GetAuditLogsQueryHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private GetAuditLogsQueryHandler CreateHandler() => new(_db);

    [Fact]
    public async Task Handle_NoFilters_ReturnsAllLogsNewestFirst()
    {
        var older = AuditLog.Create("Job", Guid.NewGuid(), AuditAction.Create, null, null, DateTimeOffset.UtcNow.AddMinutes(-5));
        var newer = AuditLog.Create("Job", Guid.NewGuid(), AuditAction.Update, null, null, DateTimeOffset.UtcNow);
        _db.AuditLogs.AddRange(older, newer);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new GetAuditLogsQuery(), CancellationToken.None);

        result.Value!.Items.Should().HaveCount(2);
        result.Value.Items.First().Id.Should().Be(newer.Id);
    }

    [Fact]
    public async Task Handle_FilteredByEntityTypeAndId_ReturnsOnlyMatchingLogs()
    {
        var jobId = Guid.NewGuid();
        _db.AuditLogs.Add(AuditLog.Create("Job", jobId, AuditAction.Create, null, null, DateTimeOffset.UtcNow));
        _db.AuditLogs.Add(AuditLog.Create("Job", Guid.NewGuid(), AuditAction.Create, null, null, DateTimeOffset.UtcNow));
        _db.AuditLogs.Add(AuditLog.Create("Candidate", jobId, AuditAction.Create, null, null, DateTimeOffset.UtcNow));
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new GetAuditLogsQuery(EntityType: "Job", EntityId: jobId), CancellationToken.None);

        result.Value!.Items.Should().ContainSingle(l => l.EntityType == "Job" && l.EntityId == jobId);
    }
}
