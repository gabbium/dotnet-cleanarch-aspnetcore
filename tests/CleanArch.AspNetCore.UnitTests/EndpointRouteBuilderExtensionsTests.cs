namespace CleanArch.AspNetCore.UnitTests;

public class EndpointRouteBuilderExtensionsTests
{
    private static int s_mapCalls;

    public class MyApi : IApi
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            Interlocked.Increment(ref s_mapCalls);
        }
    }

    [Fact]
    public void MapApis_CallsMapForAllAssemblyApis()
    {
        // Arrange
        s_mapCalls = 0;

        var builder = WebApplication.CreateBuilder();

        var app = builder.Build();

        // Act
        app.MapApis(Assembly.GetExecutingAssembly());

        // Assert
        Assert.Equal(1, s_mapCalls);
    }
}
