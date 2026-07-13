using FluentAssertions;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.Events;
using Xunit;

namespace RecruitPro.Domain.Tests.Recruitment;

public sealed class OfferTests
{
    private static readonly DateOnly Today = new(2026, 1, 1);

    private static Offer CreateOffer() => Offer.Create(Guid.NewGuid(), 2_000_000, "INR", Today);

    [Fact]
    public void Extend_WhileDraft_Succeeds()
    {
        var offer = CreateOffer();

        offer.Extend();

        offer.Status.Should().Be(OfferStatus.Extended);
    }

    [Fact]
    public void Extend_WhileDraft_RaisesOfferExtendedEvent()
    {
        var offer = CreateOffer();

        offer.Extend();

        var domainEvent = Assert.Single(offer.DomainEvents) as OfferExtendedEvent;

        domainEvent.Should().NotBeNull();
        domainEvent!.OfferId.Should().Be(offer.Id);
        domainEvent.ApplicationId.Should().Be(offer.ApplicationId);
        domainEvent.OfferedSalary.Should().Be(2_000_000);
        domainEvent.CurrencyCode.Should().Be("INR");
    }

    [Fact]
    public void Accept_WhileDraft_ThrowsOfferStateTransitionException()
    {
        var offer = CreateOffer();

        var act = offer.Accept;

        act.Should().Throw<OfferStateTransitionException>();
    }

    [Fact]
    public void Accept_WhileExtended_Succeeds()
    {
        var offer = CreateOffer();
        offer.Extend();

        offer.Accept();

        offer.Status.Should().Be(OfferStatus.Accepted);
    }

    [Fact]
    public void Reject_WhileExtended_Succeeds()
    {
        var offer = CreateOffer();
        offer.Extend();

        offer.Reject();

        offer.Status.Should().Be(OfferStatus.Rejected);
    }

    [Fact]
    public void Withdraw_WhileDraft_Succeeds()
    {
        var offer = CreateOffer();

        offer.Withdraw();

        offer.Status.Should().Be(OfferStatus.Withdrawn);
    }

    [Fact]
    public void Withdraw_AfterAccepted_ThrowsOfferStateTransitionException()
    {
        var offer = CreateOffer();
        offer.Extend();
        offer.Accept();

        var act = offer.Withdraw;

        act.Should().Throw<OfferStateTransitionException>();
    }
}
