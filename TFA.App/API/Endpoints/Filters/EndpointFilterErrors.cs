using TFA.App.Domain.Abstractions;

namespace TFA.App.API.Endpoints.Filters;

public static class EndpointFilterErrors
{
    public static Error InvalidApiKeyError  
        => new Error(nameof(InvalidApiKeyError),"Invalid API Key");
    
    public static Error InvalidOperationError
        => new Error(nameof(InvalidOperationError), "Invalid operation");
}