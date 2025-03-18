using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.App.Domain.Models;

namespace TFA.App.Database.Configuration;

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
        
        //Users
        builder.Property(x => x.OwnerId)
            .HasColumnName("TaskOwnerId")
            .IsRequired();
        
        builder.Property(x => x.CurrentOwnerId)
            .HasColumnName("TaskCurrentOwnerId")
            .IsRequired();
        
        //Subtasks
        builder.HasOne(t => t.ParentTask)
            .WithMany(t => t.Subtasks)
            .HasForeignKey(t => t.ParentTaskId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}