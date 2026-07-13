using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Offers.WithdrawOffer;

/// <summary>Deliberately does not touch the JobApplication stage: withdrawing is often an
/// administrative correction (e.g. a Draft offer that was never shown to the candidate), unlike
/// RejectOffer which represents the candidate actually declining.</summary>
public sealed class WithdrawOfferCommandHandler(IApplicationDbContext db) : IRequestHandler<WithdrawOfferCommand, Result<OfferDto>>
{
    public async Task<Result<OfferDto>> Handle(WithdrawOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await db.Offers.SingleOrDefaultAsync(o => o.Id == request.OfferId, cancellationToken);
        if (offer is null)
            return Result<OfferDto>.NotFound();

        offer.Withdraw();

        await db.SaveChangesAsync(cancellationToken);

        return Result<OfferDto>.Success(OfferDto.FromEntity(offer));
    }
}
