using FluentAssertions;
using NSubstitute;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Commands.Login;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Identity.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Identity.Commands.Login;

public sealed class LoginCommandHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IJwtTokenService _jwtTokenService = Substitute.For<IJwtTokenService>();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    public LoginCommandHandlerTests()
    {
        _dateTimeProvider.UtcNow.Returns(Now);
        _jwtTokenService.AccessTokenLifetime.Returns(TimeSpan.FromMinutes(15));
        _jwtTokenService.RefreshTokenLifetime.Returns(TimeSpan.FromDays(7));
        _jwtTokenService.GenerateAccessToken(Arg.Any<ApplicationUser>(), Arg.Any<IEnumerable<string>>()).Returns("access-token");
        _jwtTokenService.GenerateRefreshToken().Returns(("raw-refresh-token", "hashed-refresh-token"));
    }

    private LoginCommandHandler CreateHandler() => new(_db, _passwordHasher, _jwtTokenService, _dateTimeProvider);

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsTokens()
    {
        var user = ApplicationUser.Create("recruiter@coventine.com", "stored-hash", "Ada", "Recruiter");
        _db.Users.Add(user);
        await _db.SaveChangesAsync(CancellationToken.None);

        _passwordHasher.VerifyPassword("stored-hash", "correct-password").Returns(true);

        var result = await CreateHandler().Handle(new LoginCommand("recruiter@coventine.com", "correct-password"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.AccessToken.Should().Be("access-token");
        result.Value.RefreshToken.Should().Be("raw-refresh-token");
    }

    [Fact]
    public async Task Handle_WrongPassword_ReturnsUnauthorizedAndRecordsFailedAttempt()
    {
        var user = ApplicationUser.Create("recruiter@coventine.com", "stored-hash", "Ada", "Recruiter");
        _db.Users.Add(user);
        await _db.SaveChangesAsync(CancellationToken.None);

        _passwordHasher.VerifyPassword("stored-hash", "wrong-password").Returns(false);

        var result = await CreateHandler().Handle(new LoginCommand("recruiter@coventine.com", "wrong-password"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.Unauthorized);
        user.AccessFailedCount.Should().Be(1);
    }

    [Fact]
    public async Task Handle_UnknownEmail_ReturnsUnauthorizedWithoutRevealingWhichCaseItWas()
    {
        var result = await CreateHandler().Handle(new LoginCommand("nobody@coventine.com", "whatever"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.Unauthorized);
        _passwordHasher.DidNotReceive().VerifyPassword(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_LockedOutAccount_ReturnsUnauthorizedWithoutCheckingPassword()
    {
        var user = ApplicationUser.Create("recruiter@coventine.com", "stored-hash", "Ada", "Recruiter");
        for (var i = 0; i < 5; i++)
            user.RegisterFailedLoginAttempt(Now.AddMinutes(-1));
        _db.Users.Add(user);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new LoginCommand("recruiter@coventine.com", "correct-password"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.Unauthorized);
        _passwordHasher.DidNotReceive().VerifyPassword(Arg.Any<string>(), Arg.Any<string>());
    }
}
