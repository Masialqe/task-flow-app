namespace TFA.App.API.Endpoints.Configuration;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}