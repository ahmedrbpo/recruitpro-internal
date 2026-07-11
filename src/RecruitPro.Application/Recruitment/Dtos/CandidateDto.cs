using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record CandidateDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    IReadOnlyCollection<ResumeDto> Resumes)
{
    public static CandidateDto FromEntity(Candidate candidate) =>
        new(candidate.Id, candidate.FirstName, candidate.LastName, candidate.Email, candidate.Phone,
            candidate.Resumes.Select(ResumeDto.FromEntity).ToList());
}
