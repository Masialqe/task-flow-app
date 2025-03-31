using Microsoft.EntityFrameworkCore;
using TFA.App.Database.Configuration;
using TFA.App.Database.Configuration.Application;
using TFA.App.Domain.Models;
using TFA.App.Domain.Models.Projects;
using TFA.App.Domain.Models.Tasks;
using TFA.App.Domain.Models.Users;


namespace TFA.App.Database.Context;

public class ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskObject> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new TaskObjectConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}