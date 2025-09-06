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
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Message).ToArray()
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
            _ => "An unexpected error occurred"
        };

    public static string GetType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            ErrorType.Unauthorized => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
            ErrorType.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

    public static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
}