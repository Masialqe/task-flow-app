using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TFA.App.API.Endpoints.Configuration;
using TFA.App.API.Endpoints.Filters;
using TFA.App.API.Endpoints.Results;
using TFA.App.Domain.Abstractions;
using TFA.App.Domain.Models.Users;

namespace TFA.App.API.V1.Users;

public static class LoginUser
{
    public record Command(string Email, string Password) : IRequest<Result>;
    
    public sealed class Endpoint : IEndpoint
    {
        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty().MaximumLength(50);
                RuleFor(x => x.Password).NotEmpty().MaximumLength(100);
            }
        }
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users/login", async (Command command,
                IMediator mediator, CancellationToken cancellationToken) =>
            {
               var result = await mediator.Send(command, cancellationToken);

               return result.IsSuccess
                   ? SuccessResults.Ok()
                   : FailureResults.Unauthorized(result.Error);
            })
            .AddEndpointFilter<ValidationEndpointFilter<Command>>();
        }
        
    }
    public sealed class Handler(
        UserManager<UserIdentity> userManager,
        SignInManager<UserIdentity> signInManager) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, 
            CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null) return UserErrors.UserCredentialsError;
                
            var result = await signInManager.PasswordSignInAsync(user, request.Password, 
                false, false);
            if (!result.Succeeded) return UserErrors.UserCredentialsError;
            await signInManager.SignInAsync(user, isPersistent: false);
        
            return Result.Success();
        }
    }
}