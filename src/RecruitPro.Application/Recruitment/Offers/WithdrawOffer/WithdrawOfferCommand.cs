using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Offers.WithdrawOffer;

public sealed record WithdrawOfferCommand(Guid OfferId) : IRequest<Result<OfferDto>>;
