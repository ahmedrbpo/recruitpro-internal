using FluentAssertions;
using NSubstitute;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Offers.AcceptOffer;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.ValueObjects;
using Xunit;

namespace RecruitPro.Application.Tests.Recruitment.Offers;

public sealed class AcceptOfferCommandHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    public AcceptOfferCommandHandlerTests()
    {
        _dateTimeProvider.UtcNow.Returns(Now);
    }

    private AcceptOfferCommandHandler CreateHandler() => new(_db, _dateTimeProvider, _currentUserService);

    private async Task<JobApplication> CreateApplicationAtOfferStageAsync()
    {
        var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid());
        application.MoveToStage(ApplicationStage.Screening, Now, null);
        application.MoveToStage(ApplicationStage.Interview, Now, null);
        application.MoveToStage(ApplicationStage.Offer, Now, null);
        _db.Applications.Add(application);
        await _db.SaveChangesAsync(CancellationToken.None);
        return application;
    }

    [Fact]
    public async Task Handle_ExtendedOfferForApplicationAtOfferStage_AcceptsOfferAndMovesApplicationToHired()
    {
        var application = await CreateApplicationAtOfferStageAsync();
        var offer = Offer.Create(application.Id, 2_000_000, "INR", DateOnly.FromDateTime(Now.UtcDateTime));
        offer.Extend();
        _db.Offers.Add(offer);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new AcceptOfferCommand(offer.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Status.Should().Be(OfferStatus.Accepted);

        var reloadedApplication = await _db.Applications.FindAsync(application.Id);
        reloadedApplication!.Stage.Should().Be(ApplicationStage.Hired);
    }

    [Fact]
    public async Task Handle_ApplicationNotAtOfferStage_ThrowsApplicationStageTransitionException()
    {
        // Application still at Applied (never progressed) — Offer was created directly against
        // the domain in this test, bypassing CreateOfferCommandHandler's own stage-sync logic.
        var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid());
        _db.Applications.Add(application);
        var offer = Offer.Create(application.Id, 2_000_000, "INR", DateOnly.FromDateTime(Now.UtcDateTime));
        offer.Extend();
        _db.Offers.Add(offer);
        await _db.SaveChangesAsync(CancellationToken.None);

        var act = async () => await CreateHandler().Handle(new AcceptOfferCommand(offer.Id), CancellationToken.None);

        await act.Should().ThrowAsync<Domain.Common.Exceptions.ApplicationStageTransitionException>();
    }

    [Fact]
    public async Task Handle_UnknownOffer_ReturnsNotFound()
    {
        var result = await CreateHandler().Handle(new AcceptOfferCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
