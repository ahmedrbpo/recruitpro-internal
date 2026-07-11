using FluentValidation;

namespace RecruitPro.Application.Recruitment.Jobs.GetJobsPaginated;

public sealed class GetJobsPaginatedQueryValidator : AbstractValidator<GetJobsPaginatedQuery>
{
    public GetJobsPaginatedQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
