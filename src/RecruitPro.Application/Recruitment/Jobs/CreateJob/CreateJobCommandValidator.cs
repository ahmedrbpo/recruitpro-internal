using FluentValidation;

namespace RecruitPro.Application.Recruitment.Jobs.CreateJob;

public sealed class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobCommandValidator()
    {
        RuleFor(x => x.JobCode).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.EmploymentType).IsInEnum();
        RuleFor(x => x.WorkMode).IsInEnum();
        RuleFor(x => x.ExperienceMin).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ExperienceMax).GreaterThanOrEqualTo(x => x.ExperienceMin)
            .WithMessage("Experience maximum must be greater than or equal to the minimum.");
        RuleFor(x => x.CurrencyCode).NotEmpty().Length(3);

        RuleFor(x => x.SalaryMin).GreaterThanOrEqualTo(0).When(x => x.SalaryMin.HasValue);
        RuleFor(x => x.SalaryMax)
            .GreaterThanOrEqualTo(x => x.SalaryMin!.Value)
            .When(x => x.SalaryMin.HasValue && x.SalaryMax.HasValue)
            .WithMessage("Salary maximum must be greater than or equal to the minimum.");
    }
}
