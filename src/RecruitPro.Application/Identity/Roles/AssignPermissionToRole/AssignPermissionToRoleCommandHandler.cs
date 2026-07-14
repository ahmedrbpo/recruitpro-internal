using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Roles.AssignPermissionToRole;

public sealed class AssignPermissionToRoleCommandHandler(IApplicationDbContext db)
    : IRequestHandler<AssignPermissionToRoleCommand, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(AssignPermissionToRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await db.Roles.Include(r => r.RolePermissions).SingleOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        if (role is null) return Result<RoleDto>.NotFound("Role not found.");

        var permissionExists = await db.Permissions.AnyAsync(p => p.Id == request.PermissionId, cancellationToken);
        if (!permissionExists) return Result<RoleDto>.NotFound("Permission not found.");

        role.AddPermission(request.PermissionId);
        await db.SaveChangesAsync(cancellationToken);

        // Re-fetch with the Permission navigation included — see AssignRoleToUserCommandHandler
        // for why the freshly-added join row's navigation isn't populated by EF fixup here.
        var reloaded = await db.Roles.AsNoTracking()
            .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .SingleAsync(r => r.Id == request.RoleId, cancellationToken);

        return Result<RoleDto>.Success(RoleDto.FromEntity(reloaded));
    }
}
