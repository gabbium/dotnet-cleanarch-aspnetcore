namespace CleanArch.AspNetCore;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapApis(this IEndpointRouteBuilder app, Assembly assembly)
    {
        var serviceProvider = app.ServiceProvider;

        var types = assembly.DefinedTypes
            .Where(typeInfo => !typeInfo.IsAbstract && typeof(IApi).IsAssignableFrom(typeInfo))
            .OrderBy(typeInfo => typeInfo.FullName);

        foreach (var type in types)
        {
            if (ActivatorUtilities.CreateInstance(serviceProvider, type.AsType()) is IApi api)
            {
                api.Map(app);
            }
        }

        return app;
    }
}
