using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Domain.Common.Exceptions;

namespace RecruitPro.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            await WriteProblemDetails(context, StatusCodes.Status400BadRequest, "validation_failed", "One or more validation errors occurred.", errors);
        }
        catch (DomainException ex)
        {
            // A business-rule invariant was violated (e.g. Job.Publish() without a salary range).
            await WriteProblemDetails(context, StatusCodes.Status400BadRequest, "domain_rule_violation", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteProblemDetails(context, StatusCodes.Status500InternalServerError, "internal_error", "An unexpected error occurred.");
        }
    }

    private static Task WriteProblemDetails(
        HttpContext context, int statusCode, string errorCode, string title, IDictionary<string, string[]>? errors = null)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = $"https://httpstatuses.io/{statusCode}",
        };
        problemDetails.Extensions["errorCode"] = errorCode;
        if (errors is not null)
            problemDetails.Extensions["errors"] = errors;

        return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}
