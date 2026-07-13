using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Offers.AcceptOffer;

public sealed record AcceptOfferCommand(Guid OfferId) : IRequest<Result<OfferDto>>;
