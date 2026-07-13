using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Offers.GetOfferByApplicationId;

public sealed class GetOfferByApplicationIdQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetOfferByApplicationIdQuery, Result<OfferDto>>
{
    public async Task<Result<OfferDto>> Handle(GetOfferByApplicationIdQuery request, CancellationToken cancellationToken)
    {
        var offer = await db.Offers.AsNoTracking().SingleOrDefaultAsync(o => o.ApplicationId == request.ApplicationId, cancellationToken);

        return offer is null ? Result<OfferDto>.NotFound() : Result<OfferDto>.Success(OfferDto.FromEntity(offer));
    }
}
