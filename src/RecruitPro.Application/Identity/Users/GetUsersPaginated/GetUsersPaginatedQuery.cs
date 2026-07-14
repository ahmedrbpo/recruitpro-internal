using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Users.GetUsersPaginated;

public sealed record GetUsersPaginatedQuery(int Page = 1, int PageSize = 20, bool? IsActive = null)
    : IRequest<Result<PaginatedList<UserDto>>>;
