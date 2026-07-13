using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Offers.RejectOffer;

public sealed record RejectOfferCommand(Guid OfferId) : IRequest<Result<OfferDto>>;
