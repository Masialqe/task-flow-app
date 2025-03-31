namespace TFA.App.API.V1.Projects.GetProject;

public static class GetProject
{
    public sealed record Command(Guid projectId);
}