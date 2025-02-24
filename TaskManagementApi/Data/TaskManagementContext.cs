using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Data
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
            : base(options)
        { }

        public DbSet<Task> Tasks { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Label> Labels { get; set; } = null!;
        public DbSet<TaskLabel> TaskLabels { get; set; } = null!;
        public DbSet<TaskComment> TaskComments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Many-to-Many relationship for TaskLabels
            modelBuilder.Entity<TaskLabel>()
                .HasKey(tl => new { tl.TaskId, tl.LabelId });

            modelBuilder.Entity<TaskLabel>()
                .HasOne(tl => tl.Task)
                .WithMany(t => t.TaskLabels)
                .HasForeignKey(tl => tl.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TaskLabel>()
                .HasOne(tl => tl.Label)
                .WithMany(l => l.TaskLabels)
                .HasForeignKey(tl => tl.LabelId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure TaskComment relationships properly
            modelBuilder.Entity<TaskComment>()
                .HasKey(tc => tc.Id); // Use a single primary key instead of composite key

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.TaskComments)
                .HasForeignKey(tc => tc.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.User)
                .WithMany(u => u.TaskComments)
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure relationships with NO ACTION delete behavior
            modelBuilder.Entity<Task>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Label>()
                .HasIndex(l => l.Name)
                .IsUnique();
        }
    }
}