using FluentAssertions;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Identity.Entities;
using Xunit;

namespace RecruitPro.Domain.Tests.Identity;

public sealed class ApplicationUserTests
{
    private static readonly DateTimeOffset Now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public void RegisterFailedLoginAttempt_BelowThreshold_DoesNotLockOut()
    {
        var user = ApplicationUser.Create("test@coventine.com", "hash", "Test", "User");

        for (var i = 0; i < 4; i++)
            user.RegisterFailedLoginAttempt(Now);

        user.IsLockedOut(Now).Should().BeFalse();
    }

    [Fact]
    public void RegisterFailedLoginAttempt_FifthFailure_LocksOutFor15Minutes()
    {
        var user = ApplicationUser.Create("test@coventine.com", "hash", "Test", "User");

        for (var i = 0; i < 5; i++)
            user.RegisterFailedLoginAttempt(Now);

        user.IsLockedOut(Now).Should().BeTrue();
        user.IsLockedOut(Now.AddMinutes(15).AddSeconds(1)).Should().BeFalse();
    }

    [Fact]
    public void RegisterFailedLoginAttempt_RepeatedLockouts_BackOffExponentially()
    {
        var user = ApplicationUser.Create("test@coventine.com", "hash", "Test", "User");
        var now = Now;

        // First lockout: 15 minutes.
        for (var i = 0; i < 5; i++)
            user.RegisterFailedLoginAttempt(now);
        now = user.LockoutEnd!.Value;
        user.IsLockedOut(now.AddSeconds(1)).Should().BeFalse();

        // Second lockout (after the first expires): should back off to 30 minutes.
        var secondLockoutStart = now.AddSeconds(1);
        for (var i = 0; i < 5; i++)
            user.RegisterFailedLoginAttempt(secondLockoutStart);

        (user.LockoutEnd! - secondLockoutStart).Should().Be(TimeSpan.FromMinutes(30));
    }

    [Fact]
    public void RegisterFailedLoginAttempt_WhileLockedOut_ThrowsAccountLockedOutException()
    {
        var user = ApplicationUser.Create("test@coventine.com", "hash", "Test", "User");
        for (var i = 0; i < 5; i++)
            user.RegisterFailedLoginAttempt(Now);

        var act = () => user.RegisterFailedLoginAttempt(Now.AddMinutes(1));

        act.Should().Throw<AccountLockedOutException>();
    }

    [Fact]
    public void RegisterSuccessfulLogin_ResetsFailedAttemptsAndLockout()
    {
        var user = ApplicationUser.Create("test@coventine.com", "hash", "Test", "User");
        user.RegisterFailedLoginAttempt(Now);
        user.RegisterFailedLoginAttempt(Now);

        user.RegisterSuccessfulLogin();

        user.AccessFailedCount.Should().Be(0);
        user.IsLockedOut(Now).Should().BeFalse();
    }
}
