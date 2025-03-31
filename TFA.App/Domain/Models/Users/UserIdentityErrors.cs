using TFA.App.Domain.Abstractions;

namespace TFA.App.Domain.Models.Users;

public static class UserIdentityErrors
{
    public static Error CannotCreateUserError 
        => new Error(nameof(CannotCreateUserError), "Cannot create user");
}