namespace CleanArch.AspNetCore.UnitTests;

public class CustomResultsTests
{
    [Fact]
    public void Problem_WhenResultIsSuccess_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => CustomResults.Problem(result));
    }

    [Theory]
    [InlineData(ErrorType.Validation, "Bad Request", StatusCodes.Status400BadRequest, "https://tools.ietf.org/html/rfc7231#section-6.5.1")]
    [InlineData(ErrorType.NotFound, "Not Found", StatusCodes.Status404NotFound, "https://tools.ietf.org/html/rfc7231#section-6.5.4")]
    [InlineData(ErrorType.Conflict, "Conflict", StatusCodes.Status409Conflict, "https://tools.ietf.org/html/rfc7231#section-6.5.8")]
    [InlineData(ErrorType.Unauthorized, "Unauthorized", StatusCodes.Status401Unauthorized, "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1")]
    [InlineData(ErrorType.Forbidden, "Forbidden", StatusCodes.Status403Forbidden, "https://tools.ietf.org/html/rfc7231#section-6.5.3")]
    [InlineData(ErrorType.Failure, "Server failure", StatusCodes.Status500InternalServerError, "https://tools.ietf.org/html/rfc7231#section-6.6.1")]
    public void Problem_WhenResultHasError_ThenMapsErrorTypeToExpectedStatusAndLink(
        ErrorType type,
        string expectedCode,
        int expectedStatus,
        string expectedLink)
    {
        // Arrange
        var error = new Error(type, "Some description");
        var result = Result.Failure(error);

        // Act
        var httpResult = CustomResults.Problem(result);

        // Assert
        var problem = Assert.IsType<ProblemHttpResult>(httpResult);

        Assert.NotNull(problem);
        Assert.Equal(expectedStatus, problem!.StatusCode);
        Assert.Equal(expectedStatus, problem.ProblemDetails.Status);
        Assert.Equal(expectedLink, problem.ProblemDetails.Type);
        Assert.Equal(expectedCode, problem.ProblemDetails.Title);

        if (type == ErrorType.Failure)
            Assert.Equal("An unexpected error occurred", problem.ProblemDetails.Detail);
        else
            Assert.Equal("Some description", problem.ProblemDetails.Detail);
    }

    [Fact]
    public void Problem_WhenValidationErrorListIsUsed_ThenAddErrorsToExtensions()
    {
        // Arrange
        var errorList = new ValidationErrorList(
        [
            new("Email", "Invalid email"),
            new("Password", "Weak password"),
            new("Password", "Too short")
        ]);

        var result = Result.Failure(errorList);

        // Act
        var httpResult = CustomResults.Problem(result);

        // Assert
        var problem = Assert.IsType<ProblemHttpResult>(httpResult);
        var validationResult = Assert.IsType<HttpValidationProblemDetails>(problem.ProblemDetails);

        Assert.NotNull(validationResult);
        Assert.Equal(StatusCodes.Status400BadRequest, validationResult.Status);
        Assert.Equal("Bad Request", validationResult.Title);

        var fieldErrors = validationResult.Errors;

        Assert.True(fieldErrors.ContainsKey("Email"));
        Assert.Single(fieldErrors["Email"]);
        Assert.Equal("Invalid email", fieldErrors["Email"].First());

        Assert.True(fieldErrors.ContainsKey("Password"));
        Assert.Equal(2, fieldErrors["Password"].Length);
        Assert.Contains("Weak password", fieldErrors["Password"]);
        Assert.Contains("Too short", fieldErrors["Password"]);
    }
}
