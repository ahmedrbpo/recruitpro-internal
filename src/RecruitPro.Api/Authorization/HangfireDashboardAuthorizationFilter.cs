using Hangfire.Dashboard;

namespace RecruitPro.Api.Authorization;

/// <summary>Gates /hangfire behind the same permission-claim check RequirePermissionAttribute
/// uses elsewhere, rather than exposing job history/retry controls to any authenticated user.</summary>
public sealed class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        return httpContext.User.Identity?.IsAuthenticated == true
            && httpContext.User.HasClaim("permission", "Notifications.Log.View");
    }
}
