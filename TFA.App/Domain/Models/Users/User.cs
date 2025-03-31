using Microsoft.AspNetCore.Identity;
using TFA.App.Domain.Abstractions;
using TFA.App.Domain.Models.Projects;
using TFA.App.Domain.Models.Tasks;

namespace TFA.App.Domain.Models.Users;

public class User : IEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string IdentityId { get; set; } = string.Empty;
    
    public ICollection<Project> Projects { get; set; } = [];
    public ICollection<TaskObject> OwnedTasks { get; set; } = [];
    public ICollection<TaskObject> CurrentOwnedTasks { get; set; } = [];

    public User(){}

    private User(string firstName, string lastName, string email, string identityId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        IdentityId = identityId;
    }
    public static User Create(string firstName, string lastName, string email, string identityId)
        => new User(firstName, lastName, email, identityId);
}
