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
            ResultStatus.Success => new OkResult(),
            ResultStatus.Failure => new BadRequestObjectResult(result.Exception),
            ResultStatus.Forbidden => new ForbidResult(),
            ResultStatus.Unauthorized => new UnauthorizedObjectResult(result.Exception),
            ResultStatus.Invalid => new BadRequestObjectResult(result.Exception),
            ResultStatus.NotFound => new NotFoundObjectResult(result.Exception),
            ResultStatus.Conflict => new ConflictObjectResult(result.Exception),
            ResultStatus.Unsupported =>  new StatusCodeResult(501),
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
        return result.Status switch
        {
            ResultStatus.Success => new OkObjectResult(result.Value) ,
            ResultStatus.Failure => new BadRequestObjectResult(result.Exception),
            ResultStatus.Forbidden => new ForbidResult(),
            ResultStatus.Unauthorized => new UnauthorizedObjectResult(result.Exception),
            ResultStatus.Invalid => new BadRequestObjectResult(result.Exception),
            ResultStatus.NotFound => new NotFoundObjectResult(result.Exception),
            ResultStatus.Conflict => new ConflictObjectResult(result.Exception),
            ResultStatus.Unsupported =>  new StatusCodeResult(501),
            _ => throw new NotSupportedException($"Unknown result status: {result.Status}"),
        };
    }
}