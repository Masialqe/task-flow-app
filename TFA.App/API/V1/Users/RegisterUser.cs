using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TFA.App.API.Endpoints.Configuration;
using TFA.App.API.Endpoints.Filters;
using TFA.App.API.Endpoints.Results;
using TFA.App.Database.Context;
using TFA.App.Domain.Abstractions;
using TFA.App.Domain.Models.Users;
using TFA.App.Services.Abstractions;

namespace TFA.App.API.V1.Users;

public static class RegisterUser
{
    public record Command(string Email, string Firstname, 
        string Lastname, string Password) : IRequest<Result<RegisterUserResponse>>;

    public record RegisterUserResponse(string Email, string Firstname, string Lastname);
    
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Firstname).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Lastname).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(100);
        }
    }
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users/register", async (Command command,
                IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                
                return result.IsSuccess
                    ? SuccessResults.CreatedWithoutGet($"users/login", result.Value)
                    : FailureResults.Conflict(result.Error);
            })
            .AddEndpointFilter<ValidationEndpointFilter<Command>>();;
        }
    }
    
    public sealed class Handler(
        IUserService userService,
        UserManager<UserIdentity> userManager) : IRequestHandler<Command, Result<RegisterUserResponse>>
    {
        public async Task<Result<RegisterUserResponse>> Handle(Command request, 
            CancellationToken cancellationToken)
        {
            if (await userService.IsUserAlreadyRegistered(request.Email, cancellationToken)) 
                return UserErrors.UserAlreadyExistsError;
            
            var userIdentity = UserIdentity.Create(request.Firstname, request.Lastname, request.Email);
            var createIdentityResult = await userManager.CreateAsync(userIdentity, request.Password);
            if (!createIdentityResult.Succeeded) return UserIdentityErrors.CannotCreateUserError;
            
            var user = await userService.CreateAsync(request.Firstname, request.Lastname, request.Email, 
                userIdentity.Id, cancellationToken);

            return new RegisterUserResponse(user.Email, user.FirstName, user.LastName);
        }
    }
}