using FluentAssertions;
using NSubstitute;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Applications.MoveApplicationStage;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.ValueObjects;
using Xunit;

namespace RecruitPro.Application.Tests.Recruitment.Applications;

public sealed class MoveApplicationStageCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    public MoveApplicationStageCommandHandlerTests()
    {
        _dateTimeProvider.UtcNow.Returns(DateTimeOffset.UtcNow);
    }

    private MoveApplicationStageCommandHandler CreateHandler() => new(_db, _dateTimeProvider, _currentUserService);

    [Fact]
    public async Task Handle_ValidTransition_MovesStageAndRecordsHistory()
    {
        var job = Job.Create(
            jobCode: "RP-2026-000001",
            title: "Senior .NET Developer",
            description: "Full job description.",
            employmentType: EmploymentType.FullTime,
            workMode: WorkMode.Remote,
            experienceMin: 3,
            experienceMax: 6,
            currencyCode: "INR");
        var candidate = Candidate.Create("Ada", "Recruiter", "ada@example.com");
        _db.Jobs.Add(job);
        _db.Candidates.Add(candidate);
        await _db.SaveChangesAsync(CancellationToken.None);

        var application = JobApplication.Create(job.Id, candidate.Id);
        _db.Applications.Add(application);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(
            new MoveApplicationStageCommand(application.Id, ApplicationStage.Screening), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Stage.Should().Be(ApplicationStage.Screening);
        result.Value.StageHistory.Should().ContainSingle();
    }

    [Fact]
    public async Task Handle_SkippingAStage_ThrowsApplicationStageTransitionException()
    {
        var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid());
        _db.Applications.Add(application);
        await _db.SaveChangesAsync(CancellationToken.None);

        var act = async () => await CreateHandler().Handle(
            new MoveApplicationStageCommand(application.Id, ApplicationStage.Offer), CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationStageTransitionException>();
    }

    [Fact]
    public async Task Handle_UnknownApplication_ReturnsNotFound()
    {
        var result = await CreateHandler().Handle(
            new MoveApplicationStageCommand(Guid.NewGuid(), ApplicationStage.Screening), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
