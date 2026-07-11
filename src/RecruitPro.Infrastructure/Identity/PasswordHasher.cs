using Microsoft.AspNetCore.Identity;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Infrastructure.Identity;

/// <summary>ASP.NET Identity's default PBKDF2 hasher — sufficient per the blueprint's password
/// policy, no custom hashing. The wrapped hasher doesn't actually use the user argument.</summary>
public sealed class PasswordHasher : IPasswordHasher
{
    private readonly Microsoft.AspNetCore.Identity.PasswordHasher<ApplicationUser> _hasher = new();

    public string HashPassword(string password) => _hasher.HashPassword(null!, password);

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
