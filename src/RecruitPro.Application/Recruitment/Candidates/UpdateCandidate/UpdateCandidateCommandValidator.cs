using FluentValidation;

namespace RecruitPro.Application.Recruitment.Candidates.UpdateCandidate;

public sealed class UpdateCandidateCommandValidator : AbstractValidator<UpdateCandidateCommand>
{
    public UpdateCandidateCommandValidator()
    {
        RuleFor(x => x.Pan).Matches(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$").When(x => x.Pan is not null)
            .WithMessage("PAN must match the format AAAAA9999A.");
        RuleFor(x => x.TotalExperienceYears).GreaterThanOrEqualTo(0).When(x => x.TotalExperienceYears is not null);
        RuleFor(x => x.RelevantExperienceYears).GreaterThanOrEqualTo(0).When(x => x.RelevantExperienceYears is not null);
        RuleFor(x => x.StreetAddress).MaximumLength(500);
        RuleFor(x => x.City).MaximumLength(100);
        RuleFor(x => x.State).MaximumLength(100);
        RuleFor(x => x.PostalCode).MaximumLength(20);
        RuleFor(x => x.WorkLocation).MaximumLength(200);
    }
}
