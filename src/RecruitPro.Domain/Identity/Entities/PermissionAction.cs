namespace RecruitPro.Domain.Identity.Entities;

/// <summary>Descriptive categorization for Permission.Action — not consulted by the
/// authorization pipeline, which still checks the flat Permission.Name string flattened into
/// JWT claims. Useful for grouping permissions in an admin UI.</summary>
public enum PermissionAction
{
    Read,
    Create,
    Update,
    Delete,
    Approve,
    Publish,
    Export,
    Import,
    Assign,
    Manage,
}
