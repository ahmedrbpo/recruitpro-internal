using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Clients.GetClientById;

public sealed record GetClientByIdQuery(Guid ClientId) : IRequest<Result<ClientDto>>;
