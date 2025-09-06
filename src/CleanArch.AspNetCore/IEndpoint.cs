namespace CleanArch.AspNetCore;

public interface IEndpoint
{
    void Map(IEndpointRouteBuilder builder);
}
