using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets - Matching ERD 100%
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<WorkTask> Tasks { get; set; }
        public DbSet<Worklog> Worklogs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Email).HasColumnName("email").IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.FullName).HasColumnName("full_name");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                
                entity.HasIndex(e => e.Email).IsUnique();
                
                entity.HasOne(e => e.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Role Configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // WorkTask Configuration
            modelBuilder.Entity<WorkTask>(entity =>
            {
                entity.ToTable("tasks");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title").IsRequired();
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.AssigneeId).HasColumnName("assignee_id");
                entity.Property(e => e.CreatedById).HasColumnName("created_by");
                entity.Property(e => e.Deadline).HasColumnName("deadline");
                entity.Property(e => e.Priority).HasColumnName("priority").HasConversion<string>();
                entity.Property(e => e.Status).HasColumnName("status").HasConversion<string>();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.AssigneeId);
                entity.HasIndex(e => e.Deadline);

                entity.HasOne(e => e.Assignee)
                    .WithMany(u => u.AssignedTasks)
                    .HasForeignKey(e => e.AssigneeId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(u => u.CreatedTasks)
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Worklog Configuration
            modelBuilder.Entity<Worklog>(entity =>
            {
                entity.ToTable("worklogs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TaskId).HasColumnName("task_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Date).HasColumnName("log_date");
                entity.Property(e => e.HoursSpent).HasColumnName("hours_spent").HasPrecision(4, 2);
                entity.Property(e => e.Note).HasColumnName("note");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                
                entity.HasIndex(e => new { e.UserId, e.TaskId, e.Date }).IsUnique();
                entity.HasIndex(e => e.Date);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Worklogs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Task)
                    .WithMany(t => t.Worklogs)
                    .HasForeignKey(e => e.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // RefreshToken Configuration
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_tokens");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Token).HasColumnName("token").IsRequired();
                entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
                entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                
                entity.HasIndex(e => e.Token).IsUnique();
                entity.HasIndex(e => e.UserId);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
