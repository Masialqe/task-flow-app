using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TFA.App.Database.Configuration.Identity;
using TFA.App.Domain.Models.Projects;
using TFA.App.Domain.Models.Tasks;
using TFA.App.Domain.Models.Users;

namespace TFA.App.Database.Context;

public class IdentityDataContext : IdentityDbContext<UserIdentity>
{
    public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
        : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<TaskObject>();
        modelBuilder.Ignore<Project>();
        modelBuilder.Ignore<User>();
        modelBuilder.ApplyConfiguration(new UserIdentityConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
