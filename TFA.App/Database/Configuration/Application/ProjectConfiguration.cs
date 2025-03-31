using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.App.Domain.Models.Projects;

namespace TFA.App.Database.Configuration;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        
        builder.Property(x => x.Name)
            .HasColumnType("varchar(100)")
            .HasColumnName("ProjectName")
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasColumnType("varchar(500)")
            .HasColumnName("ProjectDescription")
            .IsRequired();

        builder.Property(x => x.OwnerId)
            .HasColumnName("ProjectOwnerId")
            .HasColumnType("uuid")
            .IsRequired();
        
        //Users
        builder.HasMany(x => x.Users)
            .WithMany(x => x.Projects) 
            .UsingEntity(j => j.ToTable("UserProjects")); 
        
        //Tasks
        builder.HasMany(x => x.Tasks)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}