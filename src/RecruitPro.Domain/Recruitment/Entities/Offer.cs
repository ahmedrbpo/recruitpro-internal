using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Events;

namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>1:1 extension of a JobApplication once it reaches the offer stage. Accepting an
/// offer does not itself move the application to Hired — that's a separate aggregate, and the
/// Application layer coordinates both (AcceptOfferCommandHandler) rather than one aggregate
/// reaching into another.</summary>
public sealed class Offer : BaseEntity
{
    public Guid ApplicationId { get; private set; }
    public JobApplication? Application { get; private set; }
    public decimal OfferedSalary { get; private set; }
    public string CurrencyCode { get; private set; } = default!;
    public DateOnly OfferDate { get; private set; }
    public DateOnly? JoiningDate { get; private set; }
    public DateOnly? ExpiryDate { get; private set; }
    public OfferStatus Status { get; private set; } = OfferStatus.Draft;
    public string? Notes { get; private set; }

    private Offer() { } // EF Core

    public static Offer Create(
        Guid applicationId,
        decimal offeredSalary,
        string currencyCode,
        DateOnly offerDate,
        DateOnly? joiningDate = null,
        DateOnly? expiryDate = null,
        string? notes = null) =>
        new()
        {
            ApplicationId = applicationId,
            OfferedSalary = offeredSalary,
            CurrencyCode = currencyCode,
            OfferDate = offerDate,
            JoiningDate = joiningDate,
            ExpiryDate = expiryDate,
            Notes = notes,
        };

    public void Extend()
    {
        if (Status != OfferStatus.Draft)
            throw new OfferStateTransitionException("extend", $"it is {Status}, not Draft");

        Status = OfferStatus.Extended;
        AddDomainEvent(new OfferExtendedEvent(Id, ApplicationId, OfferedSalary, CurrencyCode));
    }

    public void Accept()
    {
        if (Status != OfferStatus.Extended)
            throw new OfferStateTransitionException("accept", $"it is {Status}, not Extended");

        Status = OfferStatus.Accepted;
    }

    public void Reject()
    {
        if (Status != OfferStatus.Extended)
            throw new OfferStateTransitionException("reject", $"it is {Status}, not Extended");

        Status = OfferStatus.Rejected;
    }

    public void Withdraw()
    {
        if (Status is not (OfferStatus.Draft or OfferStatus.Extended))
            throw new OfferStateTransitionException("withdraw", $"it is {Status}, which is already final");

        Status = OfferStatus.Withdrawn;
    }
}
