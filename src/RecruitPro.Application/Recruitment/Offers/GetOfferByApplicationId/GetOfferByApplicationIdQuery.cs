using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Offers.GetOfferByApplicationId;

public sealed record GetOfferByApplicationIdQuery(Guid ApplicationId) : IRequest<Result<OfferDto>>;
