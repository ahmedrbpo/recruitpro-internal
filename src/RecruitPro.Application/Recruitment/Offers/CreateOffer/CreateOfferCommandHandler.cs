using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.ValueObjects;

namespace RecruitPro.Application.Recruitment.Offers.CreateOffer;

/// <summary>Creating an offer also moves the JobApplication to the Offer stage (if it isn't
/// there already) — keeps the two aggregates' state consistent without one reaching into the
/// other. If the application is already at Offer, MoveToStage is skipped rather than thrown.</summary>
public sealed class CreateOfferCommandHandler(IApplicationDbContext db, IDateTimeProvider dateTimeProvider, ICurrentUserService currentUserService)
    : IRequestHandler<CreateOfferCommand, Result<OfferDto>>
{
    public async Task<Result<OfferDto>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var application = await db.Applications.SingleOrDefaultAsync(a => a.Id == request.ApplicationId, cancellationToken);
        if (application is null)
            return Result<OfferDto>.NotFound("Application not found.");

        var alreadyHasOffer = await db.Offers.AnyAsync(o => o.ApplicationId == request.ApplicationId, cancellationToken);
        if (alreadyHasOffer)
            return Result<OfferDto>.Conflict("This application already has an offer.");

        var offer = Offer.Create(
            request.ApplicationId, request.OfferedSalary, request.CurrencyCode, request.OfferDate, request.JoiningDate, request.ExpiryDate, request.Notes);

        if (application.Stage != ApplicationStage.Offer)
            application.MoveToStage(ApplicationStage.Offer, dateTimeProvider.UtcNow, currentUserService.UserId);

        db.Offers.Add(offer);
        await db.SaveChangesAsync(cancellationToken);

        return Result<OfferDto>.Success(OfferDto.FromEntity(offer));
    }
}
