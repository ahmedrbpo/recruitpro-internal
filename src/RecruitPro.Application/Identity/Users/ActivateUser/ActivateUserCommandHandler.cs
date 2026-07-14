using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Users.ActivateUser;

public sealed class ActivateUserCommandHandler(IApplicationDbContext db) : IRequestHandler<ActivateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await db.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user is null) return Result<UserDto>.NotFound();

        user.Activate();
        await db.SaveChangesAsync(cancellationToken);

        return Result<UserDto>.Success(UserDto.FromEntity(user));
    }
}
