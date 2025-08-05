using Microsoft.EntityFrameworkCore;
using CompanySystem.Data.Entities;
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
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasIndex(r => r.RoleName).IsUnique();
                entity.Property(r => r.RoleName).IsRequired().HasMaxLength(50).HasColumnType("varchar(50)");
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.SerialNumber).IsUnique();
                
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255).HasColumnType("varchar(255)");
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255).HasColumnType("varchar(255)");
                entity.Property(u => u.SerialNumber).IsRequired().HasMaxLength(20).HasColumnType("varchar(20)");
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
                      .WithMany()
                      .HasForeignKey(u => u.DepartmentId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.HasIndex(d => d.DepartmentName).IsUnique();
                entity.Property(d => d.DepartmentName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(d => d.Description).HasMaxLength(500).HasColumnType("varchar(500)");
                entity.Property(d => d.CreatedDate).HasColumnType("datetime");
                entity.Property(d => d.UpdatedDate).HasColumnType("datetime");


            });

            // Configure Note entity
            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Title).IsRequired().HasMaxLength(200).HasColumnType("varchar(200)");
                entity.Property(n => n.Content).IsRequired().HasColumnType("text");
                entity.Property(n => n.CreatedDate).HasColumnType("datetime");
                entity.Property(n => n.UpdatedDate).HasColumnType("datetime");
                

            });

            // Configure MainPageContent entity
            modelBuilder.Entity<MainPageContent>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(200).HasColumnType("varchar(200)");
                entity.Property(c => c.Content).IsRequired().HasColumnType("text");
                entity.Property(c => c.CreatedDate).HasColumnType("datetime");
                entity.Property(c => c.UpdatedDate).HasColumnType("datetime");
                
                // Configure unique constraint on section name
                entity.HasIndex(c => c.SectionName).IsUnique();


            });

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.EmployeeCode).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                
                entity.Property(e => e.EmployeeCode).IsRequired().HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255).HasColumnType("varchar(255)");
                entity.Property(e => e.PhoneNumber).HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(e => e.HireDate).HasColumnType("datetime");
                entity.Property(e => e.TerminationDate).HasColumnType("datetime");
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Position).HasMaxLength(50).HasColumnType("varchar(50)");
                entity.Property(e => e.EmploymentStatus).HasMaxLength(50).HasColumnType("varchar(50)");
                entity.Property(e => e.Skills).HasColumnType("text");
                entity.Property(e => e.Experience).HasColumnType("text");
                entity.Property(e => e.ProfilePhoto).HasMaxLength(500).HasColumnType("varchar(500)");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");


            });

            // Configure Project entity
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.ProjectName).IsUnique();
                
                entity.Property(p => p.ProjectName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(p => p.Description).HasMaxLength(500).HasColumnType("varchar(500)");
                entity.Property(p => p.StartDate).HasColumnType("datetime");
                entity.Property(p => p.EndDate).HasColumnType("datetime");
                entity.Property(p => p.Status).HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(p => p.Budget).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Priority).HasMaxLength(50).HasColumnType("varchar(50)");
                entity.Property(p => p.Requirements).HasColumnType("text");
                entity.Property(p => p.Notes).HasColumnType("text");
                entity.Property(p => p.CreatedDate).HasColumnType("datetime");
                entity.Property(p => p.UpdatedDate).HasColumnType("datetime");


            });

            // Configure Task entity
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);
                
                entity.Property(t => t.TaskName).IsRequired().HasMaxLength(200).HasColumnType("varchar(200)");
                entity.Property(t => t.Description).HasColumnType("text");
                entity.Property(t => t.DueDate).HasColumnType("datetime");
                entity.Property(t => t.CompletedDate).HasColumnType("datetime");
                entity.Property(t => t.Status).HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(t => t.Priority).HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(t => t.EstimatedHours).HasColumnType("decimal(18,2)");
                entity.Property(t => t.ActualHours).HasColumnType("decimal(18,2)");
                entity.Property(t => t.Notes).HasColumnType("text");
                entity.Property(t => t.CreatedDate).HasColumnType("datetime");
                entity.Property(t => t.UpdatedDate).HasColumnType("datetime");


            });

            // Configure Company entity
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasIndex(c => c.CompanyName).IsUnique();
                entity.HasIndex(c => c.CompanyCode).IsUnique();
                
                entity.Property(c => c.CompanyName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(c => c.CompanyCode).HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(c => c.Description).HasMaxLength(500).HasColumnType("varchar(500)");
                entity.Property(c => c.Website).HasMaxLength(255).HasColumnType("varchar(255)");
                entity.Property(c => c.Email).HasMaxLength(255).HasColumnType("varchar(255)");
                entity.Property(c => c.PhoneNumber).HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(c => c.Address).HasMaxLength(500).HasColumnType("varchar(500)");
                entity.Property(c => c.City).HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(c => c.State).HasMaxLength(50).HasColumnType("varchar(50)");
                entity.Property(c => c.PostalCode).HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(c => c.Country).HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(c => c.Industry).HasMaxLength(100).HasColumnType("varchar(100)");
                entity.Property(c => c.AnnualRevenue).HasColumnType("decimal(18,2)");
                entity.Property(c => c.EmployeeCount).HasColumnType("int");
                entity.Property(c => c.CompanySize).HasMaxLength(50).HasColumnType("varchar(50)");
                entity.Property(c => c.FoundedDate).HasColumnType("datetime");
                entity.Property(c => c.CreatedDate).HasColumnType("datetime");
                entity.Property(c => c.UpdatedDate).HasColumnType("datetime");
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = Role.RoleNames.Administrator },
                new Role { Id = 2, RoleName = Role.RoleNames.HR },
                new Role { Id = 3, RoleName = Role.RoleNames.Lead },
                new Role { Id = 4, RoleName = Role.RoleNames.Employee }
            );

            // Note: Admin user will be created through a separate initialization service
            // to avoid hardcoding password hashes in the database context

            // Seed default departments
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, DepartmentName = "IT", CreatedDate = DateTime.UtcNow },
                new Department { Id = 2, DepartmentName = "HR", CreatedDate = DateTime.UtcNow },
                new Department { Id = 3, DepartmentName = "Finance", CreatedDate = DateTime.UtcNow },
                new Department { Id = 4, DepartmentName = "Operations", CreatedDate = DateTime.UtcNow }
            );

            // Note: MainPageContent will be created through a separate initialization service
            // to avoid hardcoding user references
        }
    }
} 