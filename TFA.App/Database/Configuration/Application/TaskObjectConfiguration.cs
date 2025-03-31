using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.App.Domain.Models.Tasks;

namespace TFA.App.Database.Configuration.Application;

public class TaskObjectConfiguration : IEntityTypeConfiguration<TaskObject>
{
    public void Configure(EntityTypeBuilder<TaskObject> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        
        builder.Property(x => x.Name)
            .HasColumnType("varchar(100)")
            .HasColumnName("TaskName")
            .IsRequired();
        
        builder.Property(x => x.Comment)
            .HasColumnType("varchar(500)")
            .HasColumnName("TaskComment")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("TaskCreatedAt");
        
        builder.Property(x => x.Deadline)
            .HasColumnName("TaskDeadline")
            .IsRequired(false);
        
        builder.Property(x => x.CompletedAt)
            .HasColumnName("TaskCompletedAt")
            .IsRequired(false);
        
        builder.Property(x => x.Priority)
            .HasConversion<int>()
            .HasColumnName("TaskPriority");
        
        builder.Property(x => x.State)
            .HasConversion<int>()
            .HasColumnName("TaskState");

        builder.Property(x => x.ParentTaskId)
            .HasColumnType("uuid")
            .HasColumnName("TaskParentTaskId");
        
        builder.Property(x => x.SubTaskCount)
            .HasColumnType("integer")
            .HasColumnName("TaskSubTaskCount");
    }
}