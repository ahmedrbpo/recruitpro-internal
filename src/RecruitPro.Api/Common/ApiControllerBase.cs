using Microsoft.AspNetCore.Mvc;
using RecruitPro.Application.Common.Models;

namespace RecruitPro.Api.Common;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<ApiResponse<T>> HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(ApiResponse<T>.Ok(result.Value!));

        return StatusCodeFor(result, ApiResponse<T>.Fail(ToApiError(result)));
    }

    protected ActionResult<ApiResponse<object>> HandleResult(Result result)
    {
        if (result.IsSuccess)
            return Ok(ApiResponse<object>.Ok(new { }));

        return StatusCodeFor(result, ApiResponse<object>.Fail(ToApiError(result)));
    }

    private static ApiError ToApiError(Result result) =>
        new(result.Status.ToString(), result.Error ?? "Request failed.", result.ValidationErrors);

    private ObjectResult StatusCodeFor<T>(Result result, T body) => result.Status switch
    {
        ResultStatus.NotFound => NotFound(body),
        ResultStatus.Unauthorized => Unauthorized(body),
        ResultStatus.Conflict => Conflict(body),
        ResultStatus.ValidationError => BadRequest(body),
        _ => StatusCode(StatusCodes.Status500InternalServerError, body),
    };
}
