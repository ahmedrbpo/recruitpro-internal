using FluentValidation;

namespace RecruitPro.Application.Recruitment.Candidates.ManageEmployment;

public sealed class AddCandidateEmploymentCommandValidator : AbstractValidator<AddCandidateEmploymentCommand>
{
    public AddCandidateEmploymentCommandValidator()
    {
        RuleFor(x => x.PayrollCompany).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Company).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Designation).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Location).MaximumLength(200);
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date must be on or after the start date.");
    }
}
