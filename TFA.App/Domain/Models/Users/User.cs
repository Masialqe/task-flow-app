using Microsoft.AspNetCore.Identity;

namespace TFA.App.Domain.Models.Users;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}