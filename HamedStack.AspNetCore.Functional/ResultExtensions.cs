// ReSharper disable UnusedMember.Global

using HamedStack.Functional;
using Microsoft.AspNetCore.Mvc;

namespace HamedStack.AspNetCore.Functional;

/// <summary>
/// Provides extension methods for the Result classes for ASP.NET Core functionality.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Transforms a Result instance into an IActionResult.
    /// </summary>
    /// <param name="result">The Result instance.</param>
    /// <returns>The transformed IActionResult.</returns>
    public static IActionResult ToActionResult(this Result result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => new OkResult(),
            ResultStatus.Error => new BadRequestObjectResult(result.ErrorMessage),
            ResultStatus.Forbidden => new ForbidResult(),
            ResultStatus.Unauthorized => new UnauthorizedObjectResult(result.ErrorMessage),
            ResultStatus.Invalid => new BadRequestObjectResult(result.ErrorMessage),
            ResultStatus.NotFound => new NotFoundObjectResult(result.ErrorMessage),
            ResultStatus.Conflict => new ConflictObjectResult(result.ErrorMessage),
            _ => throw new NotSupportedException($"Unknown result status: {result.Status}"),
        };
    }

    /// <summary>
    /// Transforms a Result&lt;T&gt; instance into an IActionResult.
    /// </summary>
    /// <param name="result">The Result&lt;T&gt; instance.</param>
    /// <returns>The transformed IActionResult.</returns>
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.Status == ResultStatus.Ok ? new OkObjectResult(result.Value) : ((Result)result).ToActionResult();
    }
}