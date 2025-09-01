namespace CleanArch.AspNetCore.UnitTests;

public class ResultExtensionsTests
{
    [Fact]
    public void Ok_WhenSuccess_ThenReturnsOk()
    {
        // Arrange
        var result = Result.Success(123);

        // Act
        var output = result.Ok();

        // Assert
        var ok = Assert.IsType<Ok<int>>(output);
        Assert.Equal(result.Value, ok.Value);
    }

    [Fact]
    public void Ok_WhenFailure_ThenReturnsProblem()
    {
        // Arrange
        var error = new Error("ErrorCode", "ErrorDescription", ErrorType.Failure);
        var result = Result.Failure<int>(error);

        // Act
        var output = result.Ok();

        // Assert
        Assert.IsType<ProblemHttpResult>(output);
    }

    [Fact]
    public void NoContent_WhenSuccess_ThenReturnNoContent()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var output = result.NoContent();

        // Assert
        Assert.IsType<NoContent>(output);
    }

    [Fact]
    public void NoContent_WhenFailure_ThenReturnsProblem()
    {
        // Arrange
        var error = new Error("ErrorCode", "ErrorDescription", ErrorType.Failure);
        var result = Result.Failure(error);

        // Act
        var output = result.NoContent();

        // Assert
        Assert.IsType<ProblemHttpResult>(output);
    }

    [Fact]
    public void Created_WhenSuccess_ThenReturnsCreated()
    {
        // Arrange
        var result = Result.Success(123);

        // Act
        var output = result.Created(value => $"/api/{value}");

        // Assert
        var created = Assert.IsType<Created<int>>(output);
        Assert.Equal(result.Value, created.Value);
        Assert.Equal($"/api/{result.Value}", created.Location);
    }

    [Fact]
    public void Created_WhenFailure_ThenReturnsProblem()
    {
        // Arrange
        var error = new Error("ErrorCode", "ErrorDescription", ErrorType.Failure);
        var result = Result.Failure<int>(error);

        // Act
        var output = result.Created(value => $"/api/{value}");

        // Assert
        Assert.IsType<ProblemHttpResult>(output);
    }
}
