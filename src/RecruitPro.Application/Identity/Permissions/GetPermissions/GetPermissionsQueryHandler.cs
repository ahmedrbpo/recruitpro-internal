using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Permissions.GetPermissions;

public sealed class GetPermissionsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetPermissionsQuery, Result<IReadOnlyCollection<PermissionDto>>>
{
    public async Task<Result<IReadOnlyCollection<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await db.Permissions.AsNoTracking().OrderBy(p => p.Name).ToListAsync(cancellationToken);

        IReadOnlyCollection<PermissionDto> dtos = permissions.Select(PermissionDto.FromEntity).ToList();

        return Result<IReadOnlyCollection<PermissionDto>>.Success(dtos);
    }
}
