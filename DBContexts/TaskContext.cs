using Microsoft.EntityFrameworkCore;
using task_service.Models;

namespace task_service.DBContexts;

public class TaskContext : DbContext
{
    public DbSet<TaskModel> Tasks { get; set; }
    
    public TaskContext(DbContextOptions<TaskContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskModel>().ToTable("task");
        
        //configure primary key
        modelBuilder.Entity<TaskModel>().HasKey(t => t.Id).HasName("pk_tasks");
        
        //configure index
        modelBuilder.Entity<TaskModel>().HasIndex(t => t.Id).IsUnique().HasDatabaseName("idx_tasks_id");
        
        //configure columns
        modelBuilder.Entity<TaskModel>().Property(t => t.Id).HasColumnName("id");
        modelBuilder.Entity<TaskModel>().Property(t => t.ListId).HasColumnName("list_id");
        modelBuilder.Entity<TaskModel>().Property(t => t.Name).HasColumnName("name");
        modelBuilder.Entity<TaskModel>().Property(t => t.Description).HasColumnName("description");
    }
}