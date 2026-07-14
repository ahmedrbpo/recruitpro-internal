using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Recruitment.Dtos;

/// <summary>Combines fields read-only-from-JD (Role/ClientName/RequirementId/Onboarding, all
/// derived from the linked Job/Client rather than stored on the application) with the
/// submission-time fields actually captured on the JobApplication itself.</summary>
public sealed record ApplicationSubmissionDetailsDto(
    string Role,
    string? ClientName,
    string RequirementId,
    OnboardingType? Onboarding,
    ApplicationWorkType? WorkType,
    ApplicationInterviewType? InterviewType,
    decimal? CurrentCTC,
    decimal? ExpectedCTC,
    string? UANNumber);
