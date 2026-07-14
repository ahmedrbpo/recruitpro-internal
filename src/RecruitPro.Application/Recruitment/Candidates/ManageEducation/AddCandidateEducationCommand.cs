using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Candidates.ManageEducation;

public sealed record AddCandidateEducationCommand(
    Guid CandidateId,
    string College,
    string Degree,
    string? Stream,
    EducationType Type,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Location) : IRequest<Result<CandidateEducationDto>>;
