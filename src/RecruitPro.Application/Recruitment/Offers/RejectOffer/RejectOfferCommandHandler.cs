using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.ValueObjects;

namespace RecruitPro.Application.Recruitment.Offers.RejectOffer;

/// <summary>Rejecting an offer (candidate declines) also moves the JobApplication to Rejected —
/// coordinated here across both aggregates rather than one reaching into the other.</summary>
public sealed class RejectOfferCommandHandler(IApplicationDbContext db, IDateTimeProvider dateTimeProvider, ICurrentUserService currentUserService)
    : IRequestHandler<RejectOfferCommand, Result<OfferDto>>
{
    public async Task<Result<OfferDto>> Handle(RejectOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await db.Offers.SingleOrDefaultAsync(o => o.Id == request.OfferId, cancellationToken);
        if (offer is null)
            return Result<OfferDto>.NotFound();

        offer.Reject();

        var application = await db.Applications.SingleOrDefaultAsync(a => a.Id == offer.ApplicationId, cancellationToken);
        application?.MoveToStage(ApplicationStage.Rejected, dateTimeProvider.UtcNow, currentUserService.UserId);

        await db.SaveChangesAsync(cancellationToken);

        return Result<OfferDto>.Success(OfferDto.FromEntity(offer));
    }
}
