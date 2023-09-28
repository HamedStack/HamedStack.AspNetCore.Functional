// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using HamedStack.Functional;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HamedStack.AspNetCore.Functional;

/// <summary>
/// Represents an attribute that transforms Result and Result&lt;T&gt; values to appropriate IActionResult instances.
/// </summary>
public class ResultFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Called after an action has executed.
    /// </summary>
    /// <param name="context">The action executed context.</param>
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        context.Result = context.Result switch
        {
            ObjectResult { Value: Result result } => result.ToActionResult(),
            _ => context.Result
        };
    }
}