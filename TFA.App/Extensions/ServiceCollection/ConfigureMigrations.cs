using Microsoft.EntityFrameworkCore;
using TFA.App.Database.Context;

namespace TFA.App.Extensions.ServiceCollection;

public static class ConfigureMigrations
{
    public static void ApplyMigrations(this IApplicationBuilder builder)
    {
        using IServiceScope serviceScope = builder.ApplicationServices.CreateScope();
        
        using ApplicationDataContext appContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
        using IdentityDataContext identityContext = serviceScope.ServiceProvider.GetRequiredService<IdentityDataContext>();
        
        appContext.Database.Migrate();
        identityContext.Database.Migrate();
    }
}