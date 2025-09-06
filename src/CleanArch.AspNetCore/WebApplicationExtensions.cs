namespace CleanArch.AspNetCore;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app, Assembly assembly)
    {
        var baseGroup = app.MapGroup("/api");

        var endpointTypes = assembly.DefinedTypes
            .Where(t => !t.IsAbstract && typeof(IEndpoint).IsAssignableFrom(t))
            .OrderBy(t => t.FullName);

        foreach (var type in endpointTypes)
        {
            if (ActivatorUtilities.CreateInstance(app.Services, type) is IEndpoint instance)
            {
                instance.Map(baseGroup);
            }
        }

        return app;
    }
}
