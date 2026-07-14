using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class Candidate : BaseEntity
{
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string? Phone { get; private set; }

    public Gender? Gender { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public string? Pan { get; private set; }
    public decimal? TotalExperienceYears { get; private set; }
    public decimal? RelevantExperienceYears { get; private set; }
    public string? StreetAddress { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? PostalCode { get; private set; }
    public string? WorkLocation { get; private set; }

    private readonly List<Resume> _resumes = [];
    public IReadOnlyCollection<Resume> Resumes => _resumes.AsReadOnly();

    private readonly List<CandidateEducation> _educations = [];
    public IReadOnlyCollection<CandidateEducation> Educations => _educations.AsReadOnly();

    private readonly List<CandidateEmploymentHistory> _employmentHistories = [];
    public IReadOnlyCollection<CandidateEmploymentHistory> EmploymentHistories => _employmentHistories.AsReadOnly();

    private Candidate() { } // EF Core

    public static Candidate Create(string firstName, string lastName, string email, string? phone = null) =>
        new() { FirstName = firstName, LastName = lastName, Email = email, Phone = phone };

    public void UpdatePersonalDetails(Gender? gender, DateOnly? dateOfBirth, string? pan,
        decimal? totalExperienceYears, decimal? relevantExperienceYears,
        string? streetAddress, string? city, string? state, string? postalCode, string? workLocation)
    {
        Gender = gender;
        DateOfBirth = dateOfBirth;
        Pan = pan;
        TotalExperienceYears = totalExperienceYears;
        RelevantExperienceYears = relevantExperienceYears;
        StreetAddress = streetAddress;
        City = city;
        State = state;
        PostalCode = postalCode;
        WorkLocation = workLocation;
    }
}
