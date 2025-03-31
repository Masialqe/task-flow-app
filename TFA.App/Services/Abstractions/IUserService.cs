using TFA.App.Domain.Models.Users;

namespace TFA.App.Services.Abstractions;

public interface IUserService
{
    Task<User> CreateAsync(string firstName, string lastName,
        string email, string userIdentity, CancellationToken cancellationToken = default);
    Task<User?> FindUser(string requestUserId, 
        CancellationToken cancellationToken = default);
    
    Task<User?> FindUserByIdentity(string requestIdentityId, 
        CancellationToken cancellationToken = default);

    Task<bool> IsUserAlreadyRegistered(string userEmail,
        CancellationToken cancellationToken = default);
}