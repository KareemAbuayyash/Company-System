using Microsoft.EntityFrameworkCore;
using CompanySystem.Data.Models;
using CompanySystem.Data.Enums;

namespace CompanySystem.Data.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<MainPageContent> MainPageContent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.RoleId);
                entity.HasIndex(r => r.RoleName).IsUnique();
                entity.Property(r => r.RoleName).IsRequired().HasMaxLength(50).HasColumnType("varchar(50)");
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.EmployeeId).IsUnique();
                
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255).HasColumnType("varchar(255)");
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255).HasColumnType("varchar(255)");
                entity.Property(u => u.EmployeeId).IsRequired().HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(u => u.PhoneNumber).HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(u => u.ProfilePhoto).HasMaxLength(500).HasColumnType("varchar(500)");
                entity.Property(u => u.HireDate).HasColumnType("datetime");
                entity.Property(u => u.Salary).HasColumnType("decimal(18,2)");
                entity.Property(u => u.Skills).HasColumnType("text");
                entity.Property(u => u.Experience).HasColumnType("text");
                entity.Property(u => u.CreatedDate).HasColumnType("datetime");
                entity.Property(u => u.UpdatedDate).HasColumnType("datetime");
                
                // Configure User-Role relationship
                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure User-Department relationship
                entity.HasOne(u => u.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(u => u.DepartmentId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentId);
                entity.HasIndex(d => d.DepartmentName).IsUnique();
                entity.Property(d => d.DepartmentName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(d => d.Description).HasMaxLength(500).HasColumnType("varchar(500)");
                entity.Property(d => d.CreatedDate).HasColumnType("datetime");
                entity.Property(d => d.UpdatedDate).HasColumnType("datetime");

                // Configure Department-Manager relationship
                entity.HasOne(d => d.Manager)
                      .WithMany(u => u.ManagedDepartments)
                      .HasForeignKey(d => d.ManagerId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Note entity
            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(n => n.NoteId);
                entity.Property(n => n.Title).IsRequired().HasMaxLength(200).HasColumnType("varchar(200)");
                entity.Property(n => n.Content).IsRequired().HasColumnType("text");
                entity.Property(n => n.CreatedDate).HasColumnType("datetime");
                entity.Property(n => n.UpdatedDate).HasColumnType("datetime");
                
                // Configure Note-Employee relationship
                entity.HasOne(n => n.Employee)
                      .WithMany(u => u.NotesAboutEmployee)
                      .HasForeignKey(n => n.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Configure Note-CreatedBy relationship
                entity.HasOne(n => n.CreatedBy)
                      .WithMany(u => u.NotesCreatedByEmployee)
                      .HasForeignKey(n => n.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure MainPageContent entity
            modelBuilder.Entity<MainPageContent>(entity =>
            {
                entity.HasKey(c => c.ContentId);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(200).HasColumnType("varchar(200)");
                entity.Property(c => c.Content).IsRequired().HasColumnType("text");
                entity.Property(c => c.CreatedDate).HasColumnType("datetime");
                entity.Property(c => c.UpdatedDate).HasColumnType("datetime");
                
                // Configure unique constraint on section name
                entity.HasIndex(c => c.SectionName).IsUnique();

                // Configure MainPageContent-UpdatedBy relationship
                entity.HasOne(c => c.UpdatedBy)
                      .WithMany(u => u.ContentUpdates)
                      .HasForeignKey(c => c.UpdatedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = Role.RoleNames.Administrator },
                new Role { RoleId = 2, RoleName = Role.RoleNames.HR },
                new Role { RoleId = 3, RoleName = Role.RoleNames.Lead },
                new Role { RoleId = 4, RoleName = Role.RoleNames.Employee }
            );

            // Seed initial admin user (password: Admin123!)
            // In production, use proper password hashing
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    EmployeeId = "ADMIN001",
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@company.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEBv+X3w5V5yh3Z8bX0cX8+9YzGK8h7K2L1M6N9o0P3q4R7s8T5u6V9w0X3y6Z9a2B5c8",
                    RoleId = 1,
                    HireDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                }
            );

            // Seed default departments
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "IT", CreatedDate = DateTime.UtcNow },
                new Department { DepartmentId = 2, DepartmentName = "HR", CreatedDate = DateTime.UtcNow },
                new Department { DepartmentId = 3, DepartmentName = "Finance", CreatedDate = DateTime.UtcNow },
                new Department { DepartmentId = 4, DepartmentName = "Operations", CreatedDate = DateTime.UtcNow }
            );

            // Seed default main page content
            modelBuilder.Entity<MainPageContent>().HasData(
                new MainPageContent
                {
                    ContentId = 1,
                    SectionName = SectionName.Overview,
                    Title = "Company Overview",
                    Content = "Welcome to our company management system.",
                    UpdatedById = 1,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                },
                new MainPageContent
                {
                    ContentId = 2,
                    SectionName = SectionName.AboutUs,
                    Title = "About Us",
                    Content = "We are a leading company in our industry.",
                    UpdatedById = 1,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                },
                new MainPageContent
                {
                    ContentId = 3,
                    SectionName = SectionName.Services,
                    Title = "Our Services",
                    Content = "We provide comprehensive business solutions.",
                    UpdatedById = 1,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                }
            );
        }
    }
} 