using System.Text.RegularExpressions;
using FluentValidation;

namespace RecruitPro.Application.Identity.Users.CreateUser;

/// <summary>Enforces the blueprint's password policy at the one place a password is ever
/// actually set through the API (every account before this command existed was seeded by hand,
/// bypassing any policy): minimum length 10, at least 3 of upper/lower/digit/symbol.</summary>
public sealed partial class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(10)
            .Must(HaveAtLeastThreeCharacterClasses)
            .WithMessage("Password must contain at least 3 of: uppercase letters, lowercase letters, digits, symbols.");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.LastName).MaximumLength(150);
        RuleFor(x => x.Phone).MaximumLength(30);
    }

    private static bool HaveAtLeastThreeCharacterClasses(string password)
    {
        var classesPresent = 0;
        if (UppercaseLetter().IsMatch(password)) classesPresent++;
        if (LowercaseLetter().IsMatch(password)) classesPresent++;
        if (Digit().IsMatch(password)) classesPresent++;
        if (Symbol().IsMatch(password)) classesPresent++;

        return classesPresent >= 3;
    }

    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex UppercaseLetter();

    [GeneratedRegex(@"[a-z]")]
    private static partial Regex LowercaseLetter();

    [GeneratedRegex(@"\d")]
    private static partial Regex Digit();

    [GeneratedRegex(@"[^A-Za-z0-9]")]
    private static partial Regex Symbol();
}
