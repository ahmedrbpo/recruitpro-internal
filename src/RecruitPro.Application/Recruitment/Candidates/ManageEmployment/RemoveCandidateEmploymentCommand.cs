using MediatR;
using RecruitPro.Application.Common.Models;

namespace RecruitPro.Application.Recruitment.Candidates.ManageEmployment;

public sealed record RemoveCandidateEmploymentCommand(Guid CandidateId, Guid EmploymentId) : IRequest<Result>;
