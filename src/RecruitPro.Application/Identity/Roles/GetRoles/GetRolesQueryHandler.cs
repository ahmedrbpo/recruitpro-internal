using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Roles.GetRoles;

public sealed class GetRolesQueryHandler(IApplicationDbContext db) : IRequestHandler<GetRolesQuery, Result<IReadOnlyCollection<RoleDto>>>
{
    public async Task<Result<IReadOnlyCollection<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await db.Roles.AsNoTracking()
            .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

        IReadOnlyCollection<RoleDto> dtos = roles.Select(RoleDto.FromEntity).ToList();

        return Result<IReadOnlyCollection<RoleDto>>.Success(dtos);
    }
}
