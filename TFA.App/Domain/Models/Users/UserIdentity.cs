using Microsoft.AspNetCore.Identity;

namespace TFA.App.Domain.Models.Users;

public class UserIdentity : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public static UserIdentity Create(string firstName, string lastName, string email)
        => new UserIdentity { FirstName = firstName, LastName = lastName, Email = email, UserName = email };
}