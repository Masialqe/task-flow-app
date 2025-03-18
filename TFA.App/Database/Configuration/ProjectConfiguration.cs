using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.App.Domain.Models;

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
            .IsRequired();
        
        builder.HasMany(x => x.Tasks)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(p => p.UserIds)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), 
                v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions)null) ?? new List<Guid>()
            );
        
        builder.Ignore(x => x.Users);
    }
}