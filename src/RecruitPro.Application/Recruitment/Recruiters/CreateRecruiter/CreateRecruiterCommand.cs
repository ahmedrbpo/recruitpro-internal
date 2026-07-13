using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Recruiters.CreateRecruiter;

public sealed record CreateRecruiterCommand(Guid UserId, RecruiterType Type, string? VendorCompany) : IRequest<Result<RecruiterDto>>;
