using FluentAssertions;
using NSubstitute;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Users.CreateUser;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Identity.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Identity.Users;

public sealed class CreateUserCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();

    public CreateUserCommandHandlerTests()
    {
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns(callInfo => $"hashed:{callInfo.Arg<string>()}");
    }

    private CreateUserCommandHandler CreateHandler() => new(_db, _passwordHasher);

    [Fact]
    public async Task Handle_NewEmail_CreatesUserWithHashedPassword()
    {
        var command = new CreateUserCommand("ada@coventine.com", "Sup3r$ecure!", "Ada", "Lovelace", null, null);

        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Email.Should().Be("ada@coventine.com");
        var stored = Assert.Single(_db.Users);
        stored.PasswordHash.Should().Be("hashed:Sup3r$ecure!");
    }

    [Fact]
    public async Task Handle_EmailAlreadyTaken_ReturnsConflict()
    {
        _db.Users.Add(ApplicationUser.Create("ada@coventine.com", "hash", "Ada"));
        await _db.SaveChangesAsync(CancellationToken.None);

        var command = new CreateUserCommand("ADA@coventine.com", "Sup3r$ecure!", "Ada", null, null, null);

        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.Conflict);
    }
}
