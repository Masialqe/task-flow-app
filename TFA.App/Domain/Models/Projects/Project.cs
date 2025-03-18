using System.ComponentModel.DataAnnotations.Schema;
using TFA.App.Domain.Models.Users;

namespace TFA.App.Domain.Models;

public sealed class Project
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    //Owner
    public Guid OwnerId { get; set; }
    
    //Users
    public List<Guid> UserIds { get; set; } = [];
    public List<User> Users { get; } = [];
    
    //Tasks
    public ICollection<TaskObject> Tasks { get; } = [];
}