using Microsoft.EntityFrameworkCore;
using TFA.App.Database.Context;
using TFA.App.Domain.Abstractions;
using TFA.App.Domain.Models.Users;
using TFA.App.Services.Abstractions;

namespace TFA.App.Services;

public class UserService(ApplicationDataContext context) : IUserService
{
    public async Task<User> CreateAsync(string firstName, string lastName, 
        string email, string userIdentity, CancellationToken cancellationToken = default)
    {
        var user = User.Create(firstName, lastName, email, userIdentity);
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return user;
    }
    
    /// <summary>
    /// Find user of given identifier.
    /// </summary>
    /// <param name="requestUserId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> User object if exists. </returns>
    public async Task<User?> FindUser(string requestUserId, 
        CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(x 
            => x.Id == Guid.Parse(requestUserId), cancellationToken);
    
    /// <summary>
    /// Find user by identity identifier.
    /// </summary>
    /// <param name="requestIdentityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> User object if exists.</returns>
    public async Task<User?> FindUserByIdentity(string requestIdentityId, 
        CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(x => x.IdentityId == requestIdentityId, cancellationToken);
    
    public async Task<bool> IsUserAlreadyRegistered(string userEmail,
        CancellationToken cancellationToken = default)
        => await context.Users.AnyAsync(x => x.Email == userEmail, cancellationToken);
}