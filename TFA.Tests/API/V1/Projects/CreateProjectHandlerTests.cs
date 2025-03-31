using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TFA.App.API.V1.Projects;
using TFA.App.API.V1.Projects.CreateProject;
using TFA.App.Database.Context;
using TFA.App.Domain.Models;
using TFA.App.Domain.Models.Projects;
using TFA.App.Domain.Models.Users;
using TFA.App.Services.Abstractions;

namespace TFA.Tests.API.V1.Projects;

public class CreateProjectHandlerTests
{
    private readonly ApplicationDataContext _context;
    private readonly CreateProject.Handler _handler;
    private readonly IUserService _userService;

    public CreateProjectHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDataContext>()
            .UseInMemoryDatabase("TestDB")
            .Options;

        _context = new ApplicationDataContext(options);
        _userService = Substitute.For<IUserService>();

        _handler = new CreateProject.Handler(_context, _userService);
    }
    
    [Fact]
    public async Task Handle_WhenUserDoesntExists_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var command = new CreateProject.Command("ExistingProject", "Description", userId.ToString());

        _userService.FindUserByIdentity(command.IdentityId).Returns(Task.FromResult(null as User));

        // Seed test data
        _context.Projects.Add(new Project { Name = "NotExistingProject", OwnerId = userId});
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.UserNotFoundError);
    }

    [Fact]
    public async Task Handle_WhenProjectAlreadyExists_ShouldReturnProjectAlreadyExistsError()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var command = new CreateProject.Command("ExistingProject", "Description", userId.ToString());

        var mockUser  = new User { Id = userId, IdentityId = command.IdentityId };
        _userService.FindUserByIdentity(command.IdentityId).Returns(Task.FromResult<User?>(mockUser));
        
        // Seed test data
        _context.Projects.Add(new Project { Name = "ExistingProject", OwnerId = userId});
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProjectErrors.ProjectAlreadyExistsError);
    }


    [Fact]
    public async Task Handle_WhenProjectDoesNotExist_ShouldCreateAndSaveProject()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var command = new CreateProject.Command("NewProject_NotExisting", "Description", userId.ToString());
        
        var mockUser  = new User { Id = Guid.CreateVersion7(), IdentityId = command.IdentityId };
        _userService.FindUserByIdentity(command.IdentityId).Returns(Task.FromResult<User?>(mockUser));
        
        var existingProject = await _context.Projects.AnyAsync(p => p.Name == command.Name);
        existingProject.Should().BeFalse();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var savedProject = await _context.Projects.FirstOrDefaultAsync(p => p.Name == command.Name);
        savedProject.Should().NotBeNull();
        savedProject?.Name.Should().Be(command.Name);
        
        _context.ChangeTracker.Clear();
    }
    
    [Fact]
    public async Task Handle_WhenProjectAdded_ShouldOwnerParticipateInProject()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var command = new CreateProject.Command("NewProject", "Description", userId.ToString());
        
        var mockUser  = new User { Id = Guid.CreateVersion7(), IdentityId = command.IdentityId };
        _userService.FindUserByIdentity(command.IdentityId).Returns(Task.FromResult<User?>(mockUser));
        
        var existingProject = await _context.Projects.AnyAsync(p => p.Name == command.Name);
        existingProject.Should().BeFalse();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var savedProject = await _context.Projects.Where(p => p.Name == command.Name)
            .Include(x => x.Users)
            .FirstOrDefaultAsync();
        
        savedProject?.OwnerId.Should().Be(mockUser.Id);
        savedProject?.ParticipantsCount.Should().Be(1);
        savedProject?.Users.FirstOrDefault(x => x.Id == mockUser.Id).Should().NotBeNull();
        
        _context.ChangeTracker.Clear();
    }
}
