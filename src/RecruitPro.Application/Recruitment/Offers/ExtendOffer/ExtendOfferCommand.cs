using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Offers.ExtendOffer;

public sealed record ExtendOfferCommand(Guid OfferId) : IRequest<Result<OfferDto>>;
