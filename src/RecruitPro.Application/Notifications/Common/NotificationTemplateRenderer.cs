namespace RecruitPro.Application.Notifications.Common;

/// <summary>Substitutes {{Token}} placeholders in a template string. Deliberately simple
/// (string.Replace, no conditionals/loops) — templates are short lifecycle notifications, not a
/// general-purpose templating need.</summary>
public static class NotificationTemplateRenderer
{
    public static string Render(string template, IReadOnlyDictionary<string, string> tokens)
    {
        var rendered = template;
        foreach (var (key, value) in tokens)
            rendered = rendered.Replace($"{{{{{key}}}}}", value, StringComparison.OrdinalIgnoreCase);

        return rendered;
    }
}
