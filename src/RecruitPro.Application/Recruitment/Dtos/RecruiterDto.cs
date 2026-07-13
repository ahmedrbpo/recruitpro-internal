using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record RecruiterDto(
    Guid Id,
    Guid UserId,
    RecruiterType Type,
    string? VendorCompany,
    string? PAN,
    string? Mobile,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string? Notes,
    bool IsActive)
{
    public static RecruiterDto FromEntity(Recruiter recruiter) =>
        new(
            recruiter.Id,
            recruiter.UserId,
            recruiter.Type,
            recruiter.VendorCompany,
            recruiter.PAN,
            recruiter.Mobile,
            recruiter.City,
            recruiter.State,
            recruiter.Country,
            recruiter.PostalCode,
            recruiter.Notes,
            recruiter.IsActive);
}
