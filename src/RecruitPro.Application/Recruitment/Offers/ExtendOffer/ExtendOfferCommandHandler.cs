using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Offers.ExtendOffer;

public sealed class ExtendOfferCommandHandler(IApplicationDbContext db) : IRequestHandler<ExtendOfferCommand, Result<OfferDto>>
{
    public async Task<Result<OfferDto>> Handle(ExtendOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await db.Offers.SingleOrDefaultAsync(o => o.Id == request.OfferId, cancellationToken);
        if (offer is null)
            return Result<OfferDto>.NotFound();

        offer.Extend();

        await db.SaveChangesAsync(cancellationToken);

        return Result<OfferDto>.Success(OfferDto.FromEntity(offer));
    }
}
