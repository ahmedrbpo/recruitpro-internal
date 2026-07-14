using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Identity.Users.CreateUser;

public sealed class CreateUserCommandHandler(IApplicationDbContext db, IPasswordHasher passwordHasher)
    : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        var emailTaken = await db.Users.AnyAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
        if (emailTaken)
            return Result<UserDto>.Conflict($"A user with email '{request.Email}' already exists.");

        var passwordHash = passwordHasher.HashPassword(request.Password);
        var user = ApplicationUser.Create(request.Email, passwordHash, request.FirstName, request.LastName, request.DepartmentId);

        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);

        return Result<UserDto>.Success(UserDto.FromEntity(user));
    }
}
