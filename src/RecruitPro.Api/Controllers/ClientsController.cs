using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Clients.CreateClient;
using RecruitPro.Application.Recruitment.Clients.GetClientById;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Api.Controllers;

public sealed record CreateClientRequest(
    string Name,
    string Code,
    ClientType Type,
    string? Industry,
    string? HQCountry,
    string? Email,
    string? Phone,
    string? Website,
    string? GSTNumber,
    string? CurrencyCode,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string? Notes);

[Route("api/v1/clients")]
public sealed class ClientsController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Recruitment.Client.Create")]
    public async Task<ActionResult<ApiResponse<ClientDto>>> Create(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateClientCommand(
            request.Name,
            request.Code,
            request.Type,
            request.Industry,
            request.HQCountry,
            request.Email,
            request.Phone,
            request.Website,
            request.GSTNumber,
            request.CurrencyCode,
            request.City,
            request.State,
            request.Country,
            request.PostalCode,
            request.Notes);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("Recruitment.Client.View")]
    public async Task<ActionResult<ApiResponse<ClientDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetClientByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }
}
