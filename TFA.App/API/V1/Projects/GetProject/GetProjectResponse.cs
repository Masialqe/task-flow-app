using TFA.App.Domain.Models;

namespace TFA.App.API.V1.Projects.GetProject;

public sealed record GetProjectResponse(
    Guid ProjectId,
    string ProjectName,
    string ProjectDescription,
    int ParticipantsCount,
    Guid OwnerId,
    List<UserDto> Users,
    List<TaskObjectDto> Tasks);

public sealed record UserDto(Guid Id, string Email, string FirstName, string LastName);
public sealed record TaskObjectDto(Guid Id, string Title, string Comment, 
    DateTime CreatedAt, DateTime? Deadline, TaskState State, TaskPriority Priority);
    