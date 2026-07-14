using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Candidates.ManageEmployment;

public sealed record AddCandidateEmploymentCommand(
    Guid CandidateId,
    string PayrollCompany,
    string Company,
    string Designation,
    EmploymentHistoryType Type,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Location) : IRequest<Result<CandidateEmploymentHistoryDto>>;
