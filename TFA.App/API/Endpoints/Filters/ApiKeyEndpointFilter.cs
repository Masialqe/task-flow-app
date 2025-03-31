using TFA.App.API.Endpoints.Results;

namespace TFA.App.API.Endpoints.Filters;

public sealed class ApiKeyEndpointFilter(
    IConfiguration configuration) : IEndpointFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private const string ApiKeyEnvironmentVariable = "API_KEY";
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, 
        EndpointFilterDelegate next)
    {
        var apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];
        var expectedApiKey = configuration.GetValue<string>(ApiKeyEnvironmentVariable);
        
        if(string.IsNullOrEmpty(apiKey) || 
           string.IsNullOrEmpty(expectedApiKey))
            return FailureResults.Unauthorized(EndpointFilterErrors.InvalidApiKeyError);
        
        if(!IsApiKeyValid(apiKey, expectedApiKey)) 
            return FailureResults.Unauthorized(EndpointFilterErrors.InvalidApiKeyError);
        
        return await next(context);
    }
    
    private bool IsApiKeyValid(string apiKey, string expectedApiKey)
        => apiKey.Equals(expectedApiKey, StringComparison.OrdinalIgnoreCase);
}