using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Identity.Roles.CreateRole;

public sealed class CreateRoleCommandHandler(IApplicationDbContext db) : IRequestHandler<CreateRoleCommand, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var nameTaken = await db.Roles.AnyAsync(r => r.Name == request.Name, cancellationToken);
        if (nameTaken) return Result<RoleDto>.Conflict($"A role named '{request.Name}' already exists.");

        var codeTaken = await db.Roles.AnyAsync(r => r.Code == request.Code, cancellationToken);
        if (codeTaken) return Result<RoleDto>.Conflict($"A role with code '{request.Code}' already exists.");

        // isSystem is never settable through the API — system roles are a seeding-time concern,
        // not something an admin should be able to create through this general-purpose endpoint.
        var role = Role.Create(request.Name, request.Code, request.Description);

        db.Roles.Add(role);
        await db.SaveChangesAsync(cancellationToken);

        return Result<RoleDto>.Success(RoleDto.FromEntity(role));
    }
}
