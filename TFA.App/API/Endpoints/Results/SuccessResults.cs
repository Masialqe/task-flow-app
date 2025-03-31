using TFA.App.API.Responses;
using TFA.App.Domain.Abstractions;

namespace TFA.App.API.Endpoints.Results;

public static class SuccessResults
{
    public static IResult Created<T>(string getRoute, T result) where T : IEntity 
        => Microsoft.AspNetCore.Http.Results.CreatedAtRoute($"", ApiResponse<T>.Create(
            "Successfully created", StatusCodes.Status201Created, result));
    public static IResult CreatedWithoutGet<T>(string customMethod, T result) where T : class
        => Microsoft.AspNetCore.Http.Results.Created(customMethod, ApiResponse<T>.Create(
            "Successfully created", StatusCodes.Status201Created, result));
    
    public static IResult Ok<T>(T result)
        => Microsoft.AspNetCore.Http.Results.Ok(ApiResponse<T>
            .Create("Successfully processed.",StatusCodes.Status200OK, result));
    
    public static IResult Ok()
        => Microsoft.AspNetCore.Http.Results.Ok(ApiResponse
            .Create("Successfully processed.",StatusCodes.Status200OK));

    public static IResult NotFound(string? message = null)
        => Microsoft.AspNetCore.Http.Results.NotFound(ApiResponse
            .Create(message ?? "Not found.", StatusCodes.Status404NotFound));
}

public static class FailureResults
{
    public static IResult Conflict(Error error)
        => Microsoft.AspNetCore.Http.Results.Conflict(FailureResponse.Create(
            StatusCodes.Status409Conflict, error));

    public static IResult Unauthorized(Error error)
        => Microsoft.AspNetCore.Http.Results.Ok(FailureResponse
            .Create(StatusCodes.Status401Unauthorized, error));
    
    public static IResult BadRequest(Error error)
        => Microsoft.AspNetCore.Http.Results.BadRequest(FailureResponse.Create(
            StatusCodes.Status400BadRequest, error));
    
    public static IResult BadRequest(Error[] errors)
        => Microsoft.AspNetCore.Http.Results.BadRequest(FailureResponse.Create(
            StatusCodes.Status400BadRequest, errors));
}