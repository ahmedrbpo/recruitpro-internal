using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Users.AssignRoleToUser;

public sealed class AssignRoleToUserCommandHandler(IApplicationDbContext db) : IRequestHandler<AssignRoleToUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await db.Users.Include(u => u.UserRoles).SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user is null) return Result<UserDto>.NotFound("User not found.");

        var roleExists = await db.Roles.AnyAsync(r => r.Id == request.RoleId, cancellationToken);
        if (!roleExists) return Result<UserDto>.NotFound("Role not found.");

        user.AddRole(request.RoleId);
        await db.SaveChangesAsync(cancellationToken);

        // Re-fetch with the Role navigation included — the freshly-added UserRole's Role nav
        // isn't populated by EF's in-memory fixup since the Role entity was never materialized
        // above (AnyAsync is a scalar query, not a tracked load).
        var reloaded = await db.Users.AsNoTracking()
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .SingleAsync(u => u.Id == request.UserId, cancellationToken);

        return Result<UserDto>.Success(UserDto.FromEntity(reloaded));
    }
}
