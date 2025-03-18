using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TFA.App.Database.Context;
using TFA.App.Domain.Models.Users;

namespace TFA.App.Extensions.ServiceCollection;

public static class ConfigureIdentity
{
    public static IServiceCollection ConfigureMsIdentity(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

        // services.ConfigureApplicationCookie(options =>
        // {
        //     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //     options.Cookie.HttpOnly = true;
        //     options.Cookie.SameSite = SameSiteMode.Strict;
        // });

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<IdentityDataContext>()
            .AddApiEndpoints();
        return services;
    }
}