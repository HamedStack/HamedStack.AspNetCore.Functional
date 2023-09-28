// ReSharper disable UnusedType.Global

using System.Net;
using System.Text.Json;
using HamedStack.Functional;
using Microsoft.AspNetCore.Http;

namespace HamedStack.AspNetCore.Functional;

/// <summary>
/// Middleware for handling exceptions and transforming them into Result objects.
/// </summary>
public class ResultExceptionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
    public ResultExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Processes a request to determine if it matches a known exception, and if so, produces an error response.
    /// </summary>
    /// <param name="context">The context for the current request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var result = Result.Error(ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}