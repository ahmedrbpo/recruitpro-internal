using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Candidates.UpdateCandidate;

public sealed record UpdateCandidateCommand(
    Guid CandidateId,
    Gender? Gender,
    DateOnly? DateOfBirth,
    string? Pan,
    decimal? TotalExperienceYears,
    decimal? RelevantExperienceYears,
    string? StreetAddress,
    string? City,
    string? State,
    string? PostalCode,
    string? WorkLocation) : IRequest<Result<CandidateDto>>;
