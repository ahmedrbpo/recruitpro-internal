using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Users.DeactivateUser;

public sealed class DeactivateUserCommandHandler(IApplicationDbContext db) : IRequestHandler<DeactivateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await db.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user is null) return Result<UserDto>.NotFound();

        user.Deactivate();
        await db.SaveChangesAsync(cancellationToken);

        return Result<UserDto>.Success(UserDto.FromEntity(user));
    }
}
