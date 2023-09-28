// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using HamedStack.Functional;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HamedStack.AspNetCore.Functional;

/// <summary>
/// Provides extension methods for ModelStateDictionary to transform it into Result or IActionResult.
/// </summary>
public static class ResultModelStateExtensions
{
    /// <summary>
    /// Transforms a <see cref="ModelStateDictionary"/> instance into a <see cref="Result"/> object.
    /// </summary>
    /// <param name="modelState">The ModelStateDictionary instance.</param>
    /// <param name="includeMetadata">Flag to indicate if metadata (field-specific error messages) should be included in the resulting object. Default is <c>false</c>.</param>
    /// <returns>A <see cref="Result"/> instance that represents the validation outcome.</returns>
    public static Result ToResult(this ModelStateDictionary modelState, bool includeMetadata = false)
    {
        var errors = modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var errorMessage = string.Join(" ", errors);
        var result = Result.Invalid(errorMessage);

        switch (includeMetadata)
        {
            case true:
            {
                var detailedErrors = modelState
                    .Where(m => m.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                foreach (var error in detailedErrors)
                {
                    result.AddOrUpdateMetadata(error.Key, error.Value);
                }

                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Transforms a <see cref="ModelStateDictionary"/> instance into an <see cref="IActionResult"/>.
    /// </summary>
    /// <param name="modelState">The ModelStateDictionary instance.</param>
    /// <param name="includeMetadata">Flag to indicate if metadata (field-specific error messages) should be included in the resulting object. Default is <c>false</c>.</param>
    /// <returns>An <see cref="IActionResult"/> instance that represents the validation outcome.</returns>
    public static IActionResult ToActionResult(this ModelStateDictionary modelState, bool includeMetadata = false)
    {
        return modelState.ToResult(includeMetadata).ToActionResult();
    }
}