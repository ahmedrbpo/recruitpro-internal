using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record ClientDto(
    Guid Id,
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
    string? Notes,
    bool IsActive)
{
    public static ClientDto FromEntity(Client client) =>
        new(
            client.Id,
            client.Name,
            client.Code,
            client.Type,
            client.Industry,
            client.HQCountry,
            client.Email,
            client.Phone,
            client.Website,
            client.GSTNumber,
            client.CurrencyCode,
            client.City,
            client.State,
            client.Country,
            client.PostalCode,
            client.Notes,
            client.IsActive);
}
