using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class Client : BaseEntity
{
    public Guid ClientExtId { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public ClientType Type { get; private set; }
    public string? Industry { get; private set; }
    public string? HQCountry { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Website { get; private set; }
    public string? GSTNumber { get; private set; }
    public string? CurrencyCode { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? Country { get; private set; }
    public string? PostalCode { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Client() { } // EF Core

    public static Client Create(
        string name,
        string code,
        ClientType type,
        string? industry = null,
        string? hqCountry = null,
        string? email = null,
        string? phone = null,
        string? website = null,
        string? gstNumber = null,
        string? currencyCode = null,
        string? city = null,
        string? state = null,
        string? country = null,
        string? postalCode = null,
        string? notes = null) =>
        new()
        {
            Name = name,
            Code = code,
            Type = type,
            Industry = industry,
            HQCountry = hqCountry,
            Email = email,
            Phone = phone,
            Website = website,
            GSTNumber = gstNumber,
            CurrencyCode = currencyCode,
            City = city,
            State = state,
            Country = country,
            PostalCode = postalCode,
            Notes = notes,
        };
}
