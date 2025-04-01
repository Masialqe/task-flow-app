using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TFA.App.API.Endpoints.Configuration;
using TFA.App.API.Endpoints.Results;
using TFA.App.Database.Context;
using TFA.App.Domain.Abstractions;
using TFA.App.Domain.Models;
using TFA.App.Domain.Models.Users;
using TFA.App.Extensions.Common;
using TFA.App.Services.Abstractions;

namespace TFA.App.API.V1.Projects.GetProject;

public static class GetProject
{
    public sealed record Command(Guid ProjectId, string IdentityId) : IRequest<Result<GetProjectResponse>>;
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("projects/{projectId:guid}", async (Guid projectId,
                ClaimsPrincipal claims, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var command = new Command(projectId, claims.GetUserId());

                var result = await mediator.Send(command, cancellationToken);

                return result.IsSuccess
                    ? SuccessResults.Ok(result.Value)
                    : SuccessResults.NotFound($"Project {projectId} not found for current user.");
            })
            .RequireAuthorization();
        }
    }
    
    public sealed class Handler(
        IUserService userService,
        ApplicationDataContext context) : IRequestHandler<Command, Result<GetProjectResponse>>
    {
        public async Task<Result<GetProjectResponse>> Handle(Command request, 
            CancellationToken cancellationToken)
        {
            var user = await userService.FindUserByIdentity(request.IdentityId, cancellationToken);
            if (user == null) return UserErrors.UserNotFoundError;

            var result = await context.Projects
                .AsNoTracking()
                .Include(p => p.Users)
                .Include(p => p.Tasks) 
                .Where(p => p.Id == request.ProjectId && 
                            (p.OwnerId == user.Id || p.Users.Any(u => u.Id == user.Id)))
                .Select(project => new GetProjectResponse(
                    project.Id,
                    project.Name,
                    project.Description,
                    project.ParticipantsCount,
                    project.OwnerId,
                    project.Users.Select(relatedUser => new UserDto(relatedUser.Id, relatedUser.Email, 
                        relatedUser.FirstName, relatedUser.LastName)).ToList(),
                    project.Tasks.Select(task => new TaskObjectDto(task.Id, task.Name, 
                        task.Comment, task.CreatedAt, task.Deadline, task.State, task.Priority)).ToList()))
                .SingleOrDefaultAsync(cancellationToken);
            
            return result is null ? ProjectErrors.ProjectNotFoundError : result;
        }
    }
}