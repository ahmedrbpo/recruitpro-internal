using FluentValidation;

namespace RecruitPro.Application.Recruitment.Interviews.ScheduleInterview;

public sealed class ScheduleInterviewCommandValidator : AbstractValidator<ScheduleInterviewCommand>
{
    public ScheduleInterviewCommandValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
        RuleFor(x => x.Mode).IsInEnum();
        RuleFor(x => x.Round).GreaterThanOrEqualTo(1);
        RuleFor(x => x.DurationMinutes).GreaterThan(0).When(x => x.DurationMinutes.HasValue);
    }
}
