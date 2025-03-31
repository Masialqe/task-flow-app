using TFA.App.Domain.Abstractions;
using TFA.App.Domain.Models.Projects;
using TFA.App.Domain.Models.Users;

namespace TFA.App.Domain.Models.Tasks;

public sealed class TaskObject : IEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; private set; } = string.Empty;
    public string Comment { get; private set; } = string.Empty;
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? Deadline  { get; private set; }
    
    public DateTime? CompletedAt { get; private set; }
    
    public TaskPriority Priority { get; private set; }
    public TaskState State { get; private set; } = TaskState.InProgress;
    
    public int SubTaskCount { get; private set; } = 0;
    public int? CompletedSubtasks => Subtasks.Count(x 
        => x.State == TaskState.Completed);
    
    //Owner
    public Guid OwnerId { get; set; }
    public User Owner { get; private set; } 
    
    //Current user
    public Guid CurrentOwnerId { get; set; }
    public User CurrentOwner { get; private set; }
    
    //Project
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; }

    //Subtasks
    public Guid? ParentTaskId { get; private set; }

    public ICollection<TaskObject> Subtasks { get; private set; } = [];

    public TaskObject(){}

    private TaskObject(string name, string comment, string ownerId,
        TaskPriority priority, DateTime deadline)
    {
        Name = name;
        Comment = comment;
        OwnerId = Guid.Parse(ownerId);
        Priority = priority;
    }
    
    public static TaskObject Create(string name, string comment, 
        string ownerId, TaskPriority priority, DateTime deadline)
        => new TaskObject(name, comment, ownerId, priority, deadline);

    public void CompleteTask()
    {
        if(SubTaskCount != Subtasks?.Count)
            throw new InvalidOperationException("To complete task, subtasks must be fetched.");
        
        if(CompletedSubtasks != SubTaskCount)
            throw new InvalidOperationException("To complete task, all subtasks must be completed.");
        
        State = TaskState.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void AddSubtask(TaskObject subtask)
    {
        if(Subtasks.Any(x => x.Id == subtask.Id))
            throw new InvalidOperationException("Subtask already exists.");
        Subtasks.Add(subtask);
        SubTaskCount++;
    }
} 