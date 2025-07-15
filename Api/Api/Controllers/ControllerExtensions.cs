using Microsoft.AspNetCore.Mvc;
using System.Net;
using Domain.Enums;
using Application;
using Application.Exceptions;
using Application.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Api.Controllers;

public static class ControllerExtensions {
    public static IActionResult ToActionResult<T>(this Result<T> result) {
        if (result.IsSuccess) {
            return new OkObjectResult(result.Value);
        }

        return result.Error switch {
            ValidationPipelineException vpe => new BadRequestObjectResult(new { Errors = vpe.Errors }),
            UnauthorizedAccessException => new UnauthorizedResult(),
            KeyNotFoundException or ObjectNotFoundException => new NotFoundResult(),
            ArgumentException => new BadRequestResult(),
            _ => new ObjectResult(new { ErrorMessage = "Internal server error" }) {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }

    private static readonly Regex FilterParameterExpression = new Regex("^filter_(?<column>[a-zA-Z0-9\\._]+)$");
    private static readonly string SortFieldString = "sortBy";
    private static readonly string SortDirectionString = "sortDir";

    public static KeyValuePair<string, string>[] CreateFilters(this ControllerBase controller) {
        var filters = new List<KeyValuePair<string, string>>();
        foreach (var (key, value) in controller.Request.Query) {
            var match = FilterParameterExpression.Match(key);
            if (match.Success && value.Count > 0) {
                filters.Add(new KeyValuePair<string, string>(match.Groups["column"].Value, value.ToString()));
            }
        }
        return filters.ToArray();
    }

    public static string GetSortField(this ControllerBase controller) {
        return controller.Request.Query[SortFieldString].ToString() ?? string.Empty;
    }

    public static ESortDirection GetSortDirection(this ControllerBase controller) {
        var dir = controller.Request.Query[SortDirectionString].ToString().ToLower();
        return dir switch {
            "asc" => ESortDirection.Asc,
            "desc" => ESortDirection.Desc,
            _ => ESortDirection.Desc
        };
    }
}
