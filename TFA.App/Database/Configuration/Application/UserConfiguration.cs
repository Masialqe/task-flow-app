using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.App.Domain.Models.Users;

namespace TFA.App.Database.Configuration.Application;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        
        builder.Property(x => x.FirstName)
            .HasColumnType("varchar(100)")
            .HasColumnName("UserFirstName");
        
        builder.Property(x => x.LastName)
            .HasColumnType("varchar(100)")
            .HasColumnName("UserLastName");
        
        builder.Property(x => x.Email)
            .HasColumnType("varchar(100)")
            .HasColumnName("UserEmail");
        
        builder.Property(x => x.IdentityId)
            .HasColumnType("varchar(50)")
            .HasColumnName("UserIdentityId");
        
        //Project
        builder.HasMany(x => x.Projects)
            .WithMany(x => x.Users) 
            .UsingEntity(j => j.ToTable("UserProjects")); 
        
        // Tasks
        builder.HasMany(x => x.OwnedTasks)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.CurrentOwnedTasks)
            .WithOne(x => x.CurrentOwner)
            .HasForeignKey(x => x.CurrentOwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}