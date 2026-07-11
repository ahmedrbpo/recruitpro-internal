using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Dtos;

namespace RecruitPro.Application.Recruitment.Applications.MoveApplicationStage;

public sealed class MoveApplicationStageCommandHandler(
    IApplicationDbContext db,
    IDateTimeProvider dateTimeProvider,
    ICurrentUserService currentUserService)
    : IRequestHandler<MoveApplicationStageCommand, Result<ApplicationDto>>
{
    public async Task<Result<ApplicationDto>> Handle(MoveApplicationStageCommand request, CancellationToken cancellationToken)
    {
        var application = await db.Applications.Include(a => a.StageHistory)
            .SingleOrDefaultAsync(a => a.Id == request.ApplicationId, cancellationToken);

        if (application is null)
            return Result<ApplicationDto>.NotFound();

        // Throws ApplicationStageTransitionException (a DomainException) on an invalid move —
        // left uncaught by design; ExceptionHandlingMiddleware maps it to a 400 response.
        application.MoveToStage(request.NewStage, dateTimeProvider.UtcNow, currentUserService.UserId);

        await db.SaveChangesAsync(cancellationToken);

        return Result<ApplicationDto>.Success(ApplicationDto.FromEntity(application));
    }
}
