using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class Candidate : BaseEntity
{
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string? Phone { get; private set; }

    private readonly List<Resume> _resumes = [];
    public IReadOnlyCollection<Resume> Resumes => _resumes.AsReadOnly();

    private Candidate() { } // EF Core

    public static Candidate Create(string firstName, string lastName, string email, string? phone = null) =>
        new() { FirstName = firstName, LastName = lastName, Email = email, Phone = phone };
}
