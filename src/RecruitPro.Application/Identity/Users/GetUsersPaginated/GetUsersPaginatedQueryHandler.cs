using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Users.GetUsersPaginated;

public sealed class GetUsersPaginatedQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetUsersPaginatedQuery, Result<PaginatedList<UserDto>>>
{
    public async Task<Result<PaginatedList<UserDto>>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = db.Users.AsNoTracking().Include(u => u.UserRoles).ThenInclude(ur => ur.Role).AsQueryable();

        if (request.IsActive.HasValue)
            query = query.Where(u => u.IsActive == request.IsActive.Value);

        query = query.OrderBy(u => u.Email);

        var totalCount = await query.CountAsync(cancellationToken);
        var users = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);

        var page = new PaginatedList<UserDto>(users.Select(UserDto.FromEntity).ToList(), totalCount, request.Page, request.PageSize);

        return Result<PaginatedList<UserDto>>.Success(page);
    }
}
