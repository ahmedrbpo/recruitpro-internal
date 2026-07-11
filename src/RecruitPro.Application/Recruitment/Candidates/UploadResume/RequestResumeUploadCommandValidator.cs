using FluentValidation;

namespace RecruitPro.Application.Recruitment.Candidates.UploadResume;

public sealed class RequestResumeUploadCommandValidator : AbstractValidator<RequestResumeUploadCommand>
{
    public RequestResumeUploadCommandValidator()
    {
        RuleFor(x => x.CandidateId).NotEmpty();
        RuleFor(x => x.OriginalFileName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.ContentType).NotEmpty();
        RuleFor(x => x.SizeBytes).GreaterThan(0);
    }
}
