using FluentAssertions;
using RecruitPro.Application.Identity.Users.CreateUser;
using Xunit;

namespace RecruitPro.Application.Tests.Identity.Users;

public sealed class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator = new();

    private static CreateUserCommand CommandWithPassword(string password) =>
        new("ada@coventine.com", password, "Ada", "Lovelace", null, null);

    [Fact]
    public void Validate_PasswordWithThreeCharacterClasses_Passes()
    {
        var result = _validator.Validate(CommandWithPassword("Sup3rSecure"));

        result.Errors.Should().NotContain(e => e.PropertyName == nameof(CreateUserCommand.Password));
    }

    [Fact]
    public void Validate_PasswordWithOnlyTwoCharacterClasses_Fails()
    {
        var result = _validator.Validate(CommandWithPassword("lowercaseonly123"));

        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserCommand.Password));
    }

    [Fact]
    public void Validate_PasswordShorterThanTenCharacters_Fails()
    {
        var result = _validator.Validate(CommandWithPassword("Ab1!"));

        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserCommand.Password));
    }

    [Fact]
    public void Validate_ValidCommand_Passes()
    {
        var result = _validator.Validate(CommandWithPassword("Sup3r$ecure!"));

        result.IsValid.Should().BeTrue();
    }
}
