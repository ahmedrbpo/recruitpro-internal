using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record CandidateEducationDto(
    Guid Id,
    string College,
    string Degree,
    string? Stream,
    EducationType Type,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Location)
{
    public static CandidateEducationDto FromEntity(CandidateEducation education) =>
        new(education.Id, education.College, education.Degree, education.Stream, education.Type,
            education.StartDate, education.EndDate, education.Location);
}
