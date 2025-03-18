using Microsoft.EntityFrameworkCore;
using TFA.App.Domain.Models;


namespace TFA.App.Database.Context;

public sealed class ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskObject> Tasks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDataContext).Assembly, ConfigurationFilter);
        base.OnModelCreating(modelBuilder);
    }
    
    private static bool ConfigurationFilter(Type type) =>
        type.FullName?.Contains("Database.Configurations") ?? false;
}