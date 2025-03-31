using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using TFA.App.API.V1.Users;
using TFA.App.Domain.Models.Users;

namespace TFA.Tests.API.V1.Users;

public class LoginUserHandlerTests
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly SignInManager<UserIdentity> _signInManager;
    private readonly LoginUser.Handler _handler;
    
    private LoginUser.Command request = new LoginUser.Command("nonexistent@example.com", "password");

    public LoginUserHandlerTests()
    {
        _userManager = Substitute.For<UserManager<UserIdentity>>(
            Substitute.For<IUserStore<UserIdentity>>(), null, null, null, null, null, null, null, null);

        _signInManager = Substitute.For<SignInManager<UserIdentity>>(
            _userManager, 
            Substitute.For<IHttpContextAccessor>(), 
            Substitute.For<IUserClaimsPrincipalFactory<UserIdentity>>(), 
            null, null, null, null);

        _handler = new LoginUser.Handler(_userManager, _signInManager);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserNotFound()
    {
        //Arrange
        _userManager.FindByIdAsync(Arg.Any<string>()).Returns((UserIdentity)null);
        
        //Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.UserCredentialsError);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new UserIdentity { Email = request.Email };
        
        _userManager.FindByEmailAsync(request.Email).Returns(Task.FromResult(user));
        _signInManager.PasswordSignInAsync(user, request.Password, false, false)
            .Returns(Task.FromResult(SignInResult.Failed));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.UserCredentialsError);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCredentialsAreCorrect()
    {
        // Arrange
        var user = new UserIdentity { Email = request.Email };

        _userManager.FindByEmailAsync(request.Email).Returns(Task.FromResult(user));
        _signInManager.PasswordSignInAsync(user, request.Password, false, false)
            .Returns(Task.FromResult(SignInResult.Success));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _signInManager.Received(1).SignInAsync(user, false);
    }
}