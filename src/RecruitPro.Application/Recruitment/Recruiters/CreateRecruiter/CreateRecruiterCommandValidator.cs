using FluentValidation;

namespace RecruitPro.Application.Recruitment.Recruiters.CreateRecruiter;

public sealed class CreateRecruiterCommandValidator : AbstractValidator<CreateRecruiterCommand>
{
    public CreateRecruiterCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
    }
}
