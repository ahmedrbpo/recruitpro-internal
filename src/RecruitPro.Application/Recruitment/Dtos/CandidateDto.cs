using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record CandidateDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    Gender? Gender,
    DateOnly? DateOfBirth,
    string? Pan,
    decimal? TotalExperienceYears,
    decimal? RelevantExperienceYears,
    string? StreetAddress,
    string? City,
    string? State,
    string? PostalCode,
    string? WorkLocation,
    IReadOnlyCollection<ResumeDto> Resumes,
    IReadOnlyCollection<CandidateEducationDto> Educations,
    IReadOnlyCollection<CandidateEmploymentHistoryDto> EmploymentHistory)
{
    public static CandidateDto FromEntity(Candidate candidate) =>
        new(candidate.Id, candidate.FirstName, candidate.LastName, candidate.Email, candidate.Phone,
            candidate.Gender, candidate.DateOfBirth, candidate.Pan,
            candidate.TotalExperienceYears, candidate.RelevantExperienceYears,
            candidate.StreetAddress, candidate.City, candidate.State, candidate.PostalCode, candidate.WorkLocation,
            candidate.Resumes.Select(ResumeDto.FromEntity).ToList(),
            candidate.Educations.Select(CandidateEducationDto.FromEntity).ToList(),
            candidate.EmploymentHistories.Select(CandidateEmploymentHistoryDto.FromEntity).ToList());
}
