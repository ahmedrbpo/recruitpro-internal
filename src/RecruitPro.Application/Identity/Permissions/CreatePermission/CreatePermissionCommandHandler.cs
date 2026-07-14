using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Identity.Permissions.CreatePermission;

public sealed class CreatePermissionCommandHandler(IApplicationDbContext db)
    : IRequestHandler<CreatePermissionCommand, Result<PermissionDto>>
{
    public async Task<Result<PermissionDto>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        var nameTaken = await db.Permissions.AnyAsync(p => p.Name == request.Name, cancellationToken);
        if (nameTaken) return Result<PermissionDto>.Conflict($"A permission named '{request.Name}' already exists.");

        var permission = Permission.Create(request.Name, request.Resource, request.Action, request.Description);

        db.Permissions.Add(permission);
        await db.SaveChangesAsync(cancellationToken);

        return Result<PermissionDto>.Success(PermissionDto.FromEntity(permission));
    }
}
