using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.ValueObjects;

namespace RecruitPro.Application.Recruitment.Offers.AcceptOffer;

/// <summary>Accepting an offer also moves the JobApplication to Hired — coordinated here across
/// both aggregates rather than one reaching into the other. JobApplication.MoveToStage() itself
/// enforces that Hired is only reachable from Offer, so an application not currently at the
/// Offer stage correctly fails this with a 400, not a silent no-op.</summary>
public sealed class AcceptOfferCommandHandler(IApplicationDbContext db, IDateTimeProvider dateTimeProvider, ICurrentUserService currentUserService)
    : IRequestHandler<AcceptOfferCommand, Result<OfferDto>>
{
    public async Task<Result<OfferDto>> Handle(AcceptOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await db.Offers.SingleOrDefaultAsync(o => o.Id == request.OfferId, cancellationToken);
        if (offer is null)
            return Result<OfferDto>.NotFound();

        var application = await db.Applications.SingleOrDefaultAsync(a => a.Id == offer.ApplicationId, cancellationToken);
        if (application is null)
            return Result<OfferDto>.NotFound("Application not found.");

        offer.Accept();
        application.MoveToStage(ApplicationStage.Hired, dateTimeProvider.UtcNow, currentUserService.UserId);

        await db.SaveChangesAsync(cancellationToken);

        return Result<OfferDto>.Success(OfferDto.FromEntity(offer));
    }
}
