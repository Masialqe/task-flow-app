using TFA.App.Domain.Abstractions;

namespace TFA.App.API.Responses;

public class ApiResponse
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    
    //public Dictionary<string, string>? Links { get; set; }
    
    public ApiResponse() {}
    
    public ApiResponse(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public static ApiResponse Create(string message, int statusCode)
        => new ApiResponse(message, statusCode);
}

public sealed class ApiResponse<T> : ApiResponse
{
    public T? Result { get; set; }
    public ApiResponse() {}
    
    private ApiResponse(string message, int statusCode, T? result) : base(message, statusCode)
    {
        Result = result;
    }

    public static ApiResponse<T> Create(string message, int statusCode, T? result)
        => new ApiResponse<T>(message, statusCode, result);
}

public sealed class FailureResponse : ApiResponse
{
    private static string errorMessage = "Error occurred during request";
    public List<Error> Errors { get; set; } = [];
    public FailureResponse() {}

    private FailureResponse(int statusCode, Error[] errors) : base(errorMessage, statusCode)
    {
        Errors.AddRange(errors);
    }
    
    public static FailureResponse Create(int statusCode, Error[] errors)
        => new FailureResponse(statusCode, errors);
    
    private FailureResponse(int statusCode, Error error) : base(errorMessage, statusCode)
    {
        Errors.Add(error);
    }
    
    public static FailureResponse Create(int statusCode, Error error)
        => new FailureResponse(statusCode, error);
}
