using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Clients.CreateClient;

public sealed record CreateClientCommand(
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
    string? Notes) : IRequest<Result<ClientDto>>;
