using FluentValidation;

namespace RecruitPro.Application.Recruitment.Applications.CreateApplication;

public sealed class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationCommandValidator()
    {
        RuleFor(x => x.JobId).NotEmpty();
        RuleFor(x => x.CandidateId).NotEmpty();
    }
}
