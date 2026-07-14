using FluentValidation;

namespace RecruitPro.Application.Recruitment.Candidates.ManageEducation;

public sealed class AddCandidateEducationCommandValidator : AbstractValidator<AddCandidateEducationCommand>
{
    public AddCandidateEducationCommandValidator()
    {
        RuleFor(x => x.College).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Degree).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Stream).MaximumLength(200);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Location).MaximumLength(200);
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date must be on or after the start date.");
    }
}
