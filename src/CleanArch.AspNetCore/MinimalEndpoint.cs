namespace CleanArch.AspNetCore;

public abstract class MinimalEndpoint
{
    public abstract void Map(IEndpointRouteBuilder group);
}
