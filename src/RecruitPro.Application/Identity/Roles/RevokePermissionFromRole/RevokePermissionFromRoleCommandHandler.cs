using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Roles.RevokePermissionFromRole;

public sealed class RevokePermissionFromRoleCommandHandler(IApplicationDbContext db)
    : IRequestHandler<RevokePermissionFromRoleCommand, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(RevokePermissionFromRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await db.Roles.Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .SingleOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        if (role is null) return Result<RoleDto>.NotFound();

        role.RemovePermission(request.PermissionId);
        await db.SaveChangesAsync(cancellationToken);

        return Result<RoleDto>.Success(RoleDto.FromEntity(role));
    }
}
