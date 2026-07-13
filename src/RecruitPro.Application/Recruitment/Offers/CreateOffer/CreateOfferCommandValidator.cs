using FluentValidation;

namespace RecruitPro.Application.Recruitment.Offers.CreateOffer;

public sealed class CreateOfferCommandValidator : AbstractValidator<CreateOfferCommand>
{
    public CreateOfferCommandValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
        RuleFor(x => x.OfferedSalary).GreaterThan(0);
        RuleFor(x => x.CurrencyCode).NotEmpty().Length(3);
        RuleFor(x => x.ExpiryDate).GreaterThanOrEqualTo(x => x.OfferDate).When(x => x.ExpiryDate.HasValue);
    }
}
