namespace RecruitPro.Infrastructure.Files;

public sealed class SupabaseStorageOptions
{
    public const string SectionName = "SupabaseStorage";

    public required string Url { get; init; }
    public required string ServiceRoleKey { get; init; }
    public required string Bucket { get; init; }
}
