using FluentValidation;

namespace RecruitPro.Application.Recruitment.Applications.SetSubmissionDetails;

public sealed class SetSubmissionDetailsCommandValidator : AbstractValidator<SetSubmissionDetailsCommand>
{
    public SetSubmissionDetailsCommandValidator()
    {
        RuleFor(x => x.WorkType).IsInEnum();
        RuleFor(x => x.InterviewType).IsInEnum();
        RuleFor(x => x.CurrentCTC).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ExpectedCTC).GreaterThanOrEqualTo(0);
        RuleFor(x => x.UANNumber).MaximumLength(20);
    }
}
