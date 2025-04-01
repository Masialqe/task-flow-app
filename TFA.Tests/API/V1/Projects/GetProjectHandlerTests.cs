using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TFA.App.API.V1.Projects.GetProject;
using TFA.App.Database.Context;
using TFA.App.Domain.Models;
using TFA.App.Domain.Models.Projects;
using TFA.App.Domain.Models.Users;
using TFA.App.Services.Abstractions;

namespace TFA.Tests.API.V1.Projects;

public sealed class GetProjectHandlerTests
{
    private readonly ApplicationDataContext _context;
    private readonly GetProject.Handler _handler;
    private readonly IUserService _userService;
    private readonly GetProject.Command _command;
    private readonly User _testUser;

    public GetProjectHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDataContext>()
            .UseInMemoryDatabase("TestDB")
            .Options;

        _context = new ApplicationDataContext(options);
        _userService = Substitute.For<IUserService>();

        _handler = new GetProject.Handler(_userService, _context);
        
        var identityId = Guid.CreateVersion7().ToString();
        _command = new GetProject.Command(Guid.CreateVersion7(), identityId);
        _testUser = User.Create("Test", "User", "Email", identityId);
    }
    
    [Fact]
    public async Task Handler_ShouldReturnError_WhenUserNotFound()
    {
        //Arrange
        _userService.FindUserByIdentity(_command.IdentityId).Returns(Task.FromResult(null as User));
        
        //Act
        var result = await _handler.Handle(_command, default);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.UserNotFoundError);
    }

    [Fact]
    public async Task Handler_ShouldReturnError_WhenProjectOfGivenIdDoesntExists()
    {
        //Arrange
        _userService.FindUserByIdentity(_command.IdentityId).Returns(_testUser);
        
        //Act
        var result = await _handler.Handle(_command, default);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProjectErrors.ProjectNotFoundError);
    }
    
    [Fact]
    public async Task Handler_ShouldReturnError_WhenProjectOfGivenIdIsNotRelatedWithUser()
    {
        //Arrange
        _userService.FindUserByIdentity(_command.IdentityId).Returns(_testUser);
        var notRelatedUser = User.Create("a", "b", "c", Guid.CreateVersion7().ToString());
        var newProject = Project.Create(
            "name",
            "description",
            notRelatedUser);
        newProject.Id = _command.ProjectId;
        
        _context.Projects.Add(newProject);
        _context.SaveChanges();
        
        //Act
        var result = await _handler.Handle(_command, default);
        var project = _context.Projects.FirstOrDefault(x => x.Id == _command.ProjectId);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProjectErrors.ProjectNotFoundError);
        project.Should().NotBeNull();
    }

    [Fact]
    public async Task Handler_ShouldReturnProject_WhenProjectExistsAndIsRelatedWithUser()
    {
        //Arrange
        _userService.FindUserByIdentity(_command.IdentityId).Returns(_testUser);
        var notRelatedUser = User.Create("a", "b", "c", Guid.CreateVersion7().ToString());
        var newProject = Project.Create(
            "name",
            "description",
            _testUser);
        newProject.Id = _command.ProjectId;
        
        _context.Projects.Add(newProject);
        _context.SaveChanges();
        
        //Act
        var result = await _handler.Handle(_command, default);
        var project = _context.Projects.FirstOrDefault(x => x.Id == _command.ProjectId);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        project.Should().NotBeNull();
        result.Value.ProjectId.Should().Be(_command.ProjectId);
    }
}
