using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Clients.CreateClient;

public sealed class CreateClientCommandHandler(IApplicationDbContext db) : IRequestHandler<CreateClientCommand, Result<ClientDto>>
{
    public async Task<Result<ClientDto>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var codeTaken = await db.Clients.AnyAsync(c => c.Code == request.Code, cancellationToken);
        if (codeTaken)
            return Result<ClientDto>.Conflict($"Client code '{request.Code}' is already in use.");

        var client = Client.Create(
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

        db.Clients.Add(client);
        await db.SaveChangesAsync(cancellationToken);

        return Result<ClientDto>.Success(ClientDto.FromEntity(client));
    }
}
