using Microsoft.EntityFrameworkCore;
using TFA.App.Database.Context;

namespace TFA.App.Database;

public static class ConfigureDatabaseExtension
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        services.AddDbContext<ApplicationDataContext>(options => 
            options.ConfigureNpgsql(connectionString));

        services.AddDbContext<IdentityDataContext>(options => 
            options.ConfigureNpgsql(connectionString));
        
        return services;
    }

    private static DbContextOptionsBuilder ConfigureNpgsql(this DbContextOptionsBuilder options, string connectionString)
    {
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.CommandTimeout(30);
            npgsqlOptions.EnableRetryOnFailure(3); 
        });
        
        return options;
    }
}