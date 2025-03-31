using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TFA.App.API.V1.Users;
using TFA.App.Database.Context;
using TFA.App.Domain.Models.Users;
using TFA.App.Services.Abstractions;

namespace TFA.Tests.API.V1.Users;

public class RegisterUserHandlerTests
{
    private readonly RegisterUser.Handler _handler;
    private readonly IUserService _userService;
    private readonly UserManager<UserIdentity> _userManager;
    
    private readonly RegisterUser.Command _command = new(
        "email@email.com", "first", 
        "last","password");

    public RegisterUserHandlerTests()
    {
        _userManager = Substitute.For<UserManager<UserIdentity>>(
            Substitute.For<IUserStore<UserIdentity>>(), null, null, null, null, null, null, null, null);
        _userService = Substitute.For<IUserService>();
        _handler = new RegisterUser.Handler(_userService, _userManager);
    }

    [Fact]
    public async Task Handler_ShouldReturnError_WhenUserIsAlreadyRegistered()
    {
        //Arrange
        _userService.IsUserAlreadyRegistered(_command.Email, default)
            .Returns(true);
        
        //Act
        var result = await _handler.Handle(_command, default);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.UserAlreadyExistsError);
    }

    [Fact]
    public async Task Handler_ShouldReturnError_WhenUserIdentityRegistrationFailed()
    {
        //Arrange
        _userService.IsUserAlreadyRegistered(_command.Email, default)
            .Returns(false);

        _userManager.CreateAsync(Arg.Any<UserIdentity>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed());
        
        //Act
        var result = await _handler.Handle(_command, default);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserIdentityErrors.CannotCreateUserError);
    }

    [Fact]
    public async Task Handler_ShouldCreateUser_WhenNoErrorsOccured()
    {
        // Arrange
        _userService.IsUserAlreadyRegistered(_command.Email, Arg.Any<CancellationToken>())
            .Returns(false);

        var userIdentity = new UserIdentity { Id = "123", Email = _command.Email };
    
        _userManager.CreateAsync(Arg.Any<UserIdentity>(), Arg.Any<string>())
            .Returns(Task.FromResult(IdentityResult.Success));

        _userService.CreateAsync(
                _command.Firstname, 
                _command.Lastname, 
                _command.Email, 
                Arg.Any<string>(), 
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(User.Create(
                _command.Firstname, 
                _command.Lastname, 
                _command.Email, 
                default)));

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Email.Should().Be(_command.Email);
    }

}
