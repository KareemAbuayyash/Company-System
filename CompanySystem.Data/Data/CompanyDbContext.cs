using Microsoft.EntityFrameworkCore;
using CompanySystem.Data.Models;

namespace CompanySystem.Data.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.EmployeeId).IsRequired().HasMaxLength(20);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.ProfilePhoto).HasMaxLength(255);
                entity.Property(e => e.Skills).HasMaxLength(1000);
                entity.Property(e => e.Experience).HasMaxLength(2000);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
                
                // Configure audit fields
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedById);
                entity.Property(e => e.UpdatedById);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                // Configure relationships
                entity.HasOne(e => e.Role)
                      .WithMany(e => e.Users)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure indexes
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.EmployeeId).IsUnique();
                entity.HasIndex(e => e.IsDeleted);
            });

            // Configure Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);
                
                // Configure audit fields
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedById);
                entity.Property(e => e.UpdatedById);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                // Configure indexes
                entity.HasIndex(e => e.RoleName).IsUnique();
                entity.HasIndex(e => e.IsDeleted);
            });
        }
    }
}