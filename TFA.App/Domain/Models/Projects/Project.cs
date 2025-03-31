using TFA.App.Domain.Abstractions;
using TFA.App.Domain.Models.Tasks;
using TFA.App.Domain.Models.Users;

namespace TFA.App.Domain.Models.Projects;

public sealed class Project : IEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    
    public int ParticipantsCount => Users.Count;
    
    //Owner
    public Guid OwnerId { get; set; }
    
    //Related users
    public ICollection<User> Users { get; set; } = [];
    
    //Tasks
    public ICollection<TaskObject> Tasks { get; } = [];

    public Project(){ }
    private Project(string name, string description, User owner)
    {
        Name = name;
        Description = description;
        OwnerId = owner.Id;
        Users.Add(owner);
    }
    public static Project Create(string name, string description, User owner)
        => new Project(name, description, owner);
    
}