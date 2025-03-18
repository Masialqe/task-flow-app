using TFA.App.Domain.Models.Users;

namespace TFA.App.Domain.Models;

public sealed class TaskObject
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; private set; } = string.Empty;
    public string Comment { get; private set; } = string.Empty;
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? Deadline  { get; private set; }
    
    public DateTime? CompletedAt { get; private set; }
    
    public TaskPriority Priority { get; private set; }
    public TaskState State { get; private set; } = TaskState.InProgress;
    
    //Owner
    public Guid OwnerId { get; set; }
    
    //Current user
    public Guid CurrentOwnerId { get; set; }
    
    //Project
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; }

    //Subtasks
    public Guid? ParentTaskId { get; private set; }
    public TaskObject? ParentTask { get; private set; }
    
    public ICollection<TaskObject>? Subtasks { get; private set; }
}