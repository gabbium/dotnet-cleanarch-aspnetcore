namespace CleanArch.AspNetCore;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app, Assembly assembly)
    {
        var baseGroup = app.MapGroup("/api");

        var endpointTypes = assembly.DefinedTypes
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(MinimalEndpoint)))
            .OrderBy(t => t.FullName)
            .ToArray();

        foreach (var type in endpointTypes)
        {
            if (Activator.CreateInstance(type) is MinimalEndpoint instance)
            {
                instance.Map(baseGroup);
            }
        }

        return app;
    }
}

