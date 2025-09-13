namespace CleanArch.AspNetCore;

public static class CustomResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Can't return Problem for a successful result");

        return Problem(result.Error!);
    }

    public static IResult Problem(Error error)
    {
        if (error is ValidationErrorList vel)
        {
            return Results.ValidationProblem(
                errors: ToValidationDictionary(vel),
                title: GetTitle(error),
                type: GetType(error.Type),
                statusCode: GetStatusCode(error.Type)
            );
        }

        return Results.Problem(
            title: GetTitle(error),
            detail: GetDetail(error),
            type: GetType(error.Type),
            statusCode: GetStatusCode(error.Type)
        );
    }

    public static Dictionary<string, string[]> ToValidationDictionary(ValidationErrorList errorList)
    {
        return errorList.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.Message).ToArray()
            );
    }

    public static string GetTitle(Error error) =>
        error.Type switch
        {
            ErrorType.Validation => "Bad Request",
            ErrorType.NotFound => "Not Found",
            ErrorType.Conflict => "Conflict",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.Forbidden => "Forbidden",
            ErrorType.Business => "Unprocessable Entity",
            _ => "Server failure"
        };

    public static string GetDetail(Error error) =>
        error.Type switch
        {
            ErrorType.Validation => error.Description,
            ErrorType.NotFound => error.Description,
            ErrorType.Conflict => error.Description,
            ErrorType.Unauthorized => error.Description,
            ErrorType.Forbidden => error.Description,
            ErrorType.Business => error.Description,
            _ => "An unexpected error occurred"
        };

    public static string GetType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => "https://www.rfc-editor.org/rfc/rfc9110#name-400-bad-request",
            ErrorType.NotFound => "https://www.rfc-editor.org/rfc/rfc9110#name-404-not-found",
            ErrorType.Conflict => "https://www.rfc-editor.org/rfc/rfc9110#name-409-conflict",
            ErrorType.Unauthorized => "https://www.rfc-editor.org/rfc/rfc9110#name-401-unauthorized",
            ErrorType.Forbidden => "https://www.rfc-editor.org/rfc/rfc9110#name-403-forbidden",
            ErrorType.Business => "https://www.rfc-editor.org/rfc/rfc9110#name-422-unprocessable-content",
            _ => "https://www.rfc-editor.org/rfc/rfc9110#name-500-internal-server-error"
        };

    public static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Business => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
}
