using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Candidates.GetCandidateProfile;

public sealed record GetCandidateProfileQuery(Guid CandidateId) : IRequest<Result<CandidateDto>>;
