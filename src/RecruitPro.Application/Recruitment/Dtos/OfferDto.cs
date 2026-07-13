using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record OfferDto(
    Guid Id,
    Guid ApplicationId,
    decimal OfferedSalary,
    string CurrencyCode,
    DateOnly OfferDate,
    DateOnly? JoiningDate,
    DateOnly? ExpiryDate,
    OfferStatus Status,
    string? Notes)
{
    public static OfferDto FromEntity(Offer offer) =>
        new(
            offer.Id,
            offer.ApplicationId,
            offer.OfferedSalary,
            offer.CurrencyCode,
            offer.OfferDate,
            offer.JoiningDate,
            offer.ExpiryDate,
            offer.Status,
            offer.Notes);
}
