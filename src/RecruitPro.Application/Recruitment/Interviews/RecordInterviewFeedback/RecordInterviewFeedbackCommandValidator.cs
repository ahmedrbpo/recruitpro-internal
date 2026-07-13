using FluentValidation;

namespace RecruitPro.Application.Recruitment.Interviews.RecordInterviewFeedback;

public sealed class RecordInterviewFeedbackCommandValidator : AbstractValidator<RecordInterviewFeedbackCommand>
{
    public RecordInterviewFeedbackCommandValidator()
    {
        RuleFor(x => x.InterviewId).NotEmpty();
        RuleFor(x => x.InterviewerId).NotEmpty();
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        RuleFor(x => x.Recommendation).IsInEnum();
    }
}
