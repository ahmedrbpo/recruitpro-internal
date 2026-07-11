namespace RecruitPro.Application.Common.Models;

public enum ResultStatus
{
    Success,
    NotFound,
    Unauthorized,
    Conflict,
    ValidationError,
}

public class Result
{
    public bool IsSuccess { get; }
    public ResultStatus Status { get; }
    public string? Error { get; }
    public IReadOnlyDictionary<string, string[]>? ValidationErrors { get; }

    protected Result(bool isSuccess, ResultStatus status, string? error, IReadOnlyDictionary<string, string[]>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        Status = status;
        Error = error;
        ValidationErrors = validationErrors;
    }

    public static Result Success() => new(true, ResultStatus.Success, null);
    public static Result NotFound(string? error = null) => new(false, ResultStatus.NotFound, error ?? "Resource not found.");
    public static Result Unauthorized(string? error = null) => new(false, ResultStatus.Unauthorized, error ?? "Unauthorized.");
    public static Result Conflict(string? error = null) => new(false, ResultStatus.Conflict, error ?? "Conflict.");
}

public sealed class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, ResultStatus status, string? error, IReadOnlyDictionary<string, string[]>? validationErrors = null)
        : base(isSuccess, status, error, validationErrors)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, ResultStatus.Success, null);
    public static new Result<T> NotFound(string? error = null) => new(false, default, ResultStatus.NotFound, error ?? "Resource not found.");
    public static new Result<T> Unauthorized(string? error = null) => new(false, default, ResultStatus.Unauthorized, error ?? "Unauthorized.");
    public static new Result<T> Conflict(string? error = null) => new(false, default, ResultStatus.Conflict, error ?? "Conflict.");
    public static Result<T> ValidationFailure(IReadOnlyDictionary<string, string[]> errors) =>
        new(false, default, ResultStatus.ValidationError, "Validation failed.", errors);
}
