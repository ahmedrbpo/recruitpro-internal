using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

public sealed record CandidateEmploymentHistoryDto(
    Guid Id,
    string PayrollCompany,
    string Company,
    string Designation,
    EmploymentHistoryType Type,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Location)
{
    public static CandidateEmploymentHistoryDto FromEntity(CandidateEmploymentHistory employment) =>
        new(employment.Id, employment.PayrollCompany, employment.Company, employment.Designation, employment.Type,
            employment.StartDate, employment.EndDate, employment.Location);
}
