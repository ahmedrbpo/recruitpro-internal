using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Roles.GetRoleById;

public sealed class GetRoleByIdQueryHandler(IApplicationDbContext db) : IRequestHandler<GetRoleByIdQuery, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await db.Roles.AsNoTracking()
            .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .SingleOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

        return role is null ? Result<RoleDto>.NotFound() : Result<RoleDto>.Success(RoleDto.FromEntity(role));
    }
}
