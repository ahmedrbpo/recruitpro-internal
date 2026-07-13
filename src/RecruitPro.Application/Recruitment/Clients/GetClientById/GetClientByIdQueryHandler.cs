using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Clients.GetClientById;

public sealed class GetClientByIdQueryHandler(IApplicationDbContext db) : IRequestHandler<GetClientByIdQuery, Result<ClientDto>>
{
    public async Task<Result<ClientDto>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await db.Clients.AsNoTracking().SingleOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

        return client is null ? Result<ClientDto>.NotFound() : Result<ClientDto>.Success(ClientDto.FromEntity(client));
    }
}
