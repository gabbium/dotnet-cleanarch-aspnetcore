namespace CleanArch.AspNetCore.UnitTests;

public class WebApplicationExtensionsTests
{
    private static int _mapCalls;

    public class MyMinimalEndpoint : MinimalEndpoint
    {
        public override void Map(IEndpointRouteBuilder group)
        {
            Interlocked.Increment(ref _mapCalls);
        }
    }

    [Fact]
    public void MapEndpoints_CallsMapForAllAssemblyEndpoints()
    {
        // Arrange
        _mapCalls = 0;
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        // Act
        app.MapEndpoints(Assembly.GetExecutingAssembly());

        // Assert
        Assert.Equal(1, _mapCalls);
    }
}
