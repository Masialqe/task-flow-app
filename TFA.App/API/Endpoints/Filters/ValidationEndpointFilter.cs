using FluentValidation;
using TFA.App.API.Endpoints.Results;
using TFA.App.Domain.Abstractions;

namespace TFA.App.API.Endpoints.Filters;

public sealed class ValidationEndpointFilter<TRequest>(
    IValidator<TRequest> validator) : IEndpointFilter where TRequest : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();

        if (request is null)
            return EndpointFilterErrors.InvalidOperationError;

        var validationResult = await validator.ValidateAsync(request);

        if(!validationResult.IsValid)
            return FailureResults.BadRequest(validationResult.Errors
                .Select(error => new Error(nameof(error), error.ErrorMessage)).ToArray());
        
        return await next(context);
    }
}