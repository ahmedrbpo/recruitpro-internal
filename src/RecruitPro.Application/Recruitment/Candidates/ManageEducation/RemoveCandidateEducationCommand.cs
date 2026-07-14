using MediatR;
using RecruitPro.Application.Common.Models;

namespace RecruitPro.Application.Recruitment.Candidates.ManageEducation;

public sealed record RemoveCandidateEducationCommand(Guid CandidateId, Guid EducationId) : IRequest<Result>;
