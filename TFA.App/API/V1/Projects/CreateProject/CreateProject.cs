using TFA.App.API.Endpoints.Configuration;
using TFA.App.Domain.Models.Projects;
using TFA.App.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using TFA.App.API.Endpoints.Filters;
using TFA.App.API.Endpoints.Results;
using TFA.App.Domain.Abstractions;
using TFA.App.Domain.Models.Users;
using TFA.App.Extensions.Common;
using TFA.App.Database.Context;
using System.Security.Claims;
using TFA.App.Domain.Models;
using MediatR;

namespace TFA.App.API.V1.Projects.CreateProject;

public static class CreateProject
{
    public record CreateProjectRequest(string Name, string Description);
    public record Command(string Name, string Description, string IdentityId) : IRequest<Result<Project>>;

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("projects", async (CreateProjectRequest request, 
                    ClaimsPrincipal claims, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var command = new Command(request.Name, 
                        request.Description, claims.GetUserId());
                    
                    var result = await mediator.Send(command, cancellationToken);
                    
                    //todo: add get route
                    return result.IsSuccess
                        ? SuccessResults.CreatedWithoutGet($"projects/{result.Value.Id}", result.Value.ToResponseDto())
                        : FailureResults.Conflict(result.Error);
                })
                .AddEndpointFilter<ValidationEndpointFilter<CreateProjectRequest>>()
            .RequireAuthorization();
        }
    }
    public sealed class Handler(
        ApplicationDataContext context,
        IUserService userService) : IRequestHandler<Command, Result<Project>>
    {
        public async Task<Result<Project>> Handle(Command request, 
            CancellationToken cancellationToken)
        {
            var user = await userService.FindUserByIdentity(request.IdentityId, cancellationToken);
            if (user == null) return UserErrors.UserNotFoundError;
            
            if (await IsProjectAlreadyExists(request.Name, user.Id, cancellationToken)) 
                return ProjectErrors.ProjectAlreadyExistsError;
            
            var project = Project.Create(request.Name, request.Description, user);
            await context.Projects.AddAsync(project, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            
            return project;
        }
        private async Task<bool> IsProjectAlreadyExists(
            string requestName, Guid requestUserId,
            CancellationToken cancellationToken)
        {
            return await context.Projects
                .AnyAsync(x => x.Name.ToLower() == requestName.ToLower() && 
                               x.OwnerId == requestUserId, cancellationToken);
        }
    }
}