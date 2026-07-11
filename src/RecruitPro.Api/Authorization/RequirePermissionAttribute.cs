using Microsoft.AspNetCore.Authorization;

namespace RecruitPro.Api.Authorization;

/// <summary>
/// Wraps ASP.NET Core's policy-based authorization to check a permission string flattened into
/// the JWT at issuance (e.g. "Recruitment.Job.Publish") — never a role name — so an admin can
/// redefine what a role can do without a code change.
/// </summary>
public sealed class RequirePermissionAttribute(string permission) : AuthorizeAttribute($"{PolicyPrefix}{permission}")
{
    public const string PolicyPrefix = "Permission:";
}
