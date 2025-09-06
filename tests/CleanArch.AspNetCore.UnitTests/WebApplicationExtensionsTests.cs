namespace CleanArch.AspNetCore.UnitTests;

public class WebApplicationExtensionsTests
{
    private static int s_mapCalls;

    public class MyMinimalEndpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            Interlocked.Increment(ref s_mapCalls);
        }
    }

    [Fact]
    public void MapEndpoints_CallsMapForAllAssemblyEndpoints()
    {
        // Arrange
        s_mapCalls = 0;

        var builder = WebApplication.CreateBuilder();

        var app = builder.Build();

        // Act
        app.MapEndpoints(Assembly.GetExecutingAssembly());

        // Assert
        Assert.Equal(1, s_mapCalls);
    }
}
