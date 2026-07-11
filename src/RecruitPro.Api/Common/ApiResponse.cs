namespace RecruitPro.Api.Common;

public sealed record ApiError(string Code, string Message, IReadOnlyDictionary<string, string[]>? Details = null);

public sealed record ApiResponse<T>(bool Success, T? Data, ApiError? Error, object? Meta = null)
{
    public static ApiResponse<T> Ok(T data, object? meta = null) => new(true, data, null, meta);
    public static ApiResponse<T> Fail(ApiError error) => new(false, default, error);
}
