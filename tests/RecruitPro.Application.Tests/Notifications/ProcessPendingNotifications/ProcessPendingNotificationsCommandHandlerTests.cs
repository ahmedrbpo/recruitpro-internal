using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Notifications.ProcessPendingNotifications;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Notifications.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Notifications.ProcessPendingNotifications;

public sealed class ProcessPendingNotificationsCommandHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();
    private readonly IEmailService _emailService = Substitute.For<IEmailService>();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    public ProcessPendingNotificationsCommandHandlerTests()
    {
        _dateTimeProvider.UtcNow.Returns(Now);
    }

    private ProcessPendingNotificationsCommandHandler CreateHandler() => new(_db, _emailService, _dateTimeProvider);

    [Fact]
    public async Task Handle_PendingLog_SendsEmailAndMarksSent()
    {
        var log = NotificationLog.Create(NotificationChannel.Email, "a@example.com", "A", "CODE", "Subject", "Body", null, null);
        _db.NotificationLogs.Add(log);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new ProcessPendingNotificationsCommand(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(1);
        await _emailService.Received(1).SendAsync("a@example.com", "A", "Subject", "Body", Arg.Any<CancellationToken>());

        var reloaded = await _db.NotificationLogs.FindAsync(log.Id);
        reloaded!.Status.Should().Be(NotificationStatus.Sent);
        reloaded.SentAt.Should().Be(Now);
        reloaded.Attempts.Should().Be(1);
    }

    [Fact]
    public async Task Handle_SendThrows_MarksFailedWithError()
    {
        var log = NotificationLog.Create(NotificationChannel.Email, "bad@example.com", null, "CODE", "Subject", "Body", null, null);
        _db.NotificationLogs.Add(log);
        await _db.SaveChangesAsync(CancellationToken.None);

        _emailService.SendAsync("bad@example.com", null, "Subject", "Body", Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("SendGrid rejected the request."));

        await CreateHandler().Handle(new ProcessPendingNotificationsCommand(), CancellationToken.None);

        var reloaded = await _db.NotificationLogs.FindAsync(log.Id);
        reloaded!.Status.Should().Be(NotificationStatus.Failed);
        reloaded.LastError.Should().Be("SendGrid rejected the request.");
        reloaded.Attempts.Should().Be(1);
    }

    [Fact]
    public async Task Handle_AlreadySentLog_IsIgnored()
    {
        var log = NotificationLog.Create(NotificationChannel.Email, "a@example.com", null, "CODE", "Subject", "Body", null, null);
        log.MarkSent(Now);
        _db.NotificationLogs.Add(log);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new ProcessPendingNotificationsCommand(), CancellationToken.None);

        result.Value.Should().Be(0);
        await _emailService.DidNotReceive().SendAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
