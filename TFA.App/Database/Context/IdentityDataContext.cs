using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TFA.App.Domain.Models.Users;

namespace TFA.App.Database.Context;

public class IdentityDataContext : IdentityDbContext<User>
{
    public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
        : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDataContext).Assembly, ConfigurationFilter);
        base.OnModelCreating(modelBuilder);
    }
    
    private static bool ConfigurationFilter(Type type) =>
        type.FullName?.Contains("Database.Configurations") ?? false;
}
