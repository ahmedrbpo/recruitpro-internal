using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Jobs.SetOnboarding;

public sealed record SetOnboardingCommand(Guid JobId, OnboardingType Onboarding) : IRequest<Result<JobDto>>;
