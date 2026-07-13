using RecruitPro.Domain.Common;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>Wraps an ApplicationUser with recruiter-specific attributes (type, vendor details,
/// contact info). Job.RecruiterId points here, not directly at ApplicationUser, so a vendor
/// recruiter with no login can still be assigned to a job.</summary>
public sealed class Recruiter : BaseEntity
{
    public Guid RecruiterExtId { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public ApplicationUser? User { get; private set; }
    public RecruiterType Type { get; private set; }
    public string? VendorCompany { get; private set; }
    public string? PAN { get; private set; }
    public string? Mobile { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? Country { get; private set; }
    public string? PostalCode { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Recruiter() { } // EF Core

    public static Recruiter Create(Guid userId, RecruiterType type, string? vendorCompany = null) =>
        new() { UserId = userId, Type = type, VendorCompany = vendorCompany };
}
