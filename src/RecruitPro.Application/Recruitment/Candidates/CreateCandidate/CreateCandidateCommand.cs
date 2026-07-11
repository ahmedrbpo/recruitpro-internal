using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Candidates.CreateCandidate;

public sealed record CreateCandidateCommand(string FirstName, string LastName, string Email, string? Phone)
    : IRequest<Result<CandidateDto>>;
