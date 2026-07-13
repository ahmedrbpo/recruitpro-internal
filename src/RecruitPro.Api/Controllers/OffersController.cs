using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Application.Recruitment.Offers.AcceptOffer;
using RecruitPro.Application.Recruitment.Offers.CreateOffer;
using RecruitPro.Application.Recruitment.Offers.ExtendOffer;
using RecruitPro.Application.Recruitment.Offers.RejectOffer;
using RecruitPro.Application.Recruitment.Offers.WithdrawOffer;

namespace RecruitPro.Api.Controllers;

public sealed record CreateOfferRequest(
    Guid ApplicationId, decimal OfferedSalary, string CurrencyCode, DateOnly OfferDate, DateOnly? JoiningDate, DateOnly? ExpiryDate, string? Notes);

[Route("api/v1/offers")]
public sealed class OffersController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Recruitment.Offer.Create")]
    public async Task<ActionResult<ApiResponse<OfferDto>>> Create(CreateOfferRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateOfferCommand(
            request.ApplicationId, request.OfferedSalary, request.CurrencyCode, request.OfferDate, request.JoiningDate, request.ExpiryDate, request.Notes);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/extend")]
    [RequirePermission("Recruitment.Offer.Manage")]
    public async Task<ActionResult<ApiResponse<OfferDto>>> Extend(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ExtendOfferCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/accept")]
    [RequirePermission("Recruitment.Offer.Manage")]
    public async Task<ActionResult<ApiResponse<OfferDto>>> Accept(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new AcceptOfferCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/reject")]
    [RequirePermission("Recruitment.Offer.Manage")]
    public async Task<ActionResult<ApiResponse<OfferDto>>> Reject(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RejectOfferCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/withdraw")]
    [RequirePermission("Recruitment.Offer.Manage")]
    public async Task<ActionResult<ApiResponse<OfferDto>>> Withdraw(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new WithdrawOfferCommand(id), cancellationToken);
        return HandleResult(result);
    }
}
