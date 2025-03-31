using TFA.App.Domain.Models.Projects;

namespace TFA.App.API.V1.Projects.CreateProject;

public record CreateProjectResponse(Guid Id, string Name, string Description, Guid OwnerId);

public static class CreateProjectResponseExtension
{
    public static CreateProjectResponse ToResponseDto(this Project project) => 
        new CreateProjectResponse(project.Id, project.Name, project.Description, project.OwnerId);
}