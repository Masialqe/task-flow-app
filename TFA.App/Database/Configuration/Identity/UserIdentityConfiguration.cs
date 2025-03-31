using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.App.Domain.Models.Users;

namespace TFA.App.Database.Configuration.Identity;

public class UserIdentityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FirstName)
            .HasColumnType("varchar(100)")
            .HasColumnName("UserFirstName");
        
        builder.Property(x => x.LastName)
            .HasColumnType("varchar(100)")
            .HasColumnName("UserLastName");
    }
}