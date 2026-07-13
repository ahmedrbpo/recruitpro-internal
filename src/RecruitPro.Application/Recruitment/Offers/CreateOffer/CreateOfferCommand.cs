using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Offers.CreateOffer;

public sealed record CreateOfferCommand(
    Guid ApplicationId,
    decimal OfferedSalary,
    string CurrencyCode,
    DateOnly OfferDate,
    DateOnly? JoiningDate,
    DateOnly? ExpiryDate,
    string? Notes) : IRequest<Result<OfferDto>>;
