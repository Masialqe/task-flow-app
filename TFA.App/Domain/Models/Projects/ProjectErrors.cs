using TFA.App.Domain.Abstractions;

namespace TFA.App.Domain.Models;

public static class ProjectErrors
{
    public static Error ProjectAlreadyExistsError 
        => new(nameof(ProjectAlreadyExistsError),"Project already exists");
    public static Error ProjectNotFoundError
        => new(nameof(ProjectNotFoundError),"Project not found");
    
    public static Error UserDoesntSignedForProjectError
     => new(nameof(UserDoesntSignedForProjectError),"User doesn't sign for project");
}