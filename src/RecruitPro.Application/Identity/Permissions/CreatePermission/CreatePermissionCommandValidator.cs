using FluentValidation;

namespace RecruitPro.Application.Identity.Permissions.CreatePermission;

public sealed class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Resource).MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
