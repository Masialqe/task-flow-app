using TFA.App.Domain.Abstractions;

namespace TFA.App.Domain.Models.Users;

public static class UserErrors
{
    public static Error UserNotFoundError
        => new(nameof(UserNotFoundError),"User not found");
    
    public static Error UserAlreadyExistsError
        => new(nameof(UserAlreadyExistsError),"User already exists");
    
    public static Error UserCredentialsError
        => new(nameof(UserCredentialsError),"User credentials are invalid");
    
}