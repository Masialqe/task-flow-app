using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TFA.App.Domain.Models.Users;

namespace TFA.App.Database.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FirstName)
            .HasColumnType("nvarchar(100)")
            .HasColumnName("UserFirstName");
        
        builder.Property(x => x.LastName)
            .HasColumnType("nvarchar(100)")
            .HasColumnName("UserLastName");
    }
}