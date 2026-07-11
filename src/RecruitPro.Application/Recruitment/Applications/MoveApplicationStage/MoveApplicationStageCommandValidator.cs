using FluentValidation;

namespace RecruitPro.Application.Recruitment.Applications.MoveApplicationStage;

public sealed class MoveApplicationStageCommandValidator : AbstractValidator<MoveApplicationStageCommand>
{
    public MoveApplicationStageCommandValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
        RuleFor(x => x.NewStage).NotEmpty();
    }
}
