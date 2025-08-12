// CompanySystem.Data/Data/CompanyDbContext.cs
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
        public DbSet<Department> Departments { get; set; }
        public DbSet<MainPageContent> MainPageContents { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department configuration
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.DepartmentName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                
                entity.HasIndex(e => e.DepartmentName).IsUnique();
            });

            // Role configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                
                entity.HasIndex(e => e.RoleName).IsUnique();
            });

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.ProfilePhoto).HasMaxLength(500);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Skills).HasColumnType("TEXT");
                entity.Property(e => e.Experience).HasColumnType("TEXT");
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                
                entity.HasIndex(e => e.SerialNumber).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.RoleId);
                entity.HasIndex(e => e.DepartmentId);
                entity.HasIndex(e => e.IsActive);

                // Foreign key relationships
                entity.HasOne(e => e.Role)
                    .WithMany()
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Department)
                    .WithMany()
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // MainPageContent configuration
            modelBuilder.Entity<MainPageContent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.SectionName).IsRequired();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired().HasColumnType("TEXT");
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                
                entity.HasIndex(e => e.SectionName).IsUnique();

                // Foreign key relationship
                entity.HasOne(e => e.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.UpdatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Note configuration
            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.NoteType).IsRequired();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired().HasColumnType("TEXT");
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                
                entity.HasIndex(e => e.EmployeeId);
                entity.HasIndex(e => e.NoteType);
                entity.HasIndex(e => e.CreatedById);

                // Foreign key relationships
                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Query filters for soft delete
            modelBuilder.Entity<Department>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<MainPageContent>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Note>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new Department
                {
                    Id = 1,
                    DepartmentName = "Human Resources",
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Department
                {
                    Id = 2,
                    DepartmentName = "Information Technology",
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Department
                {
                    Id = 3,
                    DepartmentName = "Finance",
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Department
                {
                    Id = 4,
                    DepartmentName = "Operations",
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    RoleName = Role.RoleNames.Administrator,
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Role
                {
                    Id = 2,
                    RoleName = Role.RoleNames.HR,
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Role
                {
                    Id = 3,
                    RoleName = Role.RoleNames.Lead,
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Role
                {
                    Id = 4,
                    RoleName = Role.RoleNames.Employee,
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );

            // Seed MainPageContent
            modelBuilder.Entity<MainPageContent>().HasData(
                new MainPageContent
                {
                    Id = 1,
                    SectionName = Enums.SectionName.Overview,
                    Title = "Welcome to Our Company",
                    Content = "We are a leading company in our industry, committed to excellence and innovation.",
                    UpdatedById = 1, // Will reference admin user
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MainPageContent
                {
                    Id = 2,
                    SectionName = Enums.SectionName.AboutUs,
                    Title = "About Our Company",
                    Content = "Founded in 2020, we have been providing exceptional services to our clients worldwide.",
                    UpdatedById = 1, // Will reference admin user
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MainPageContent
                {
                    Id = 3,
                    SectionName = Enums.SectionName.Services,
                    Title = "Our Professional Services",
                    Content = "We offer a comprehensive range of professional services including consulting and development.",
                    UpdatedById = 1, // Will reference admin user
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );

            // Seed Admin User (password will be set by initialization service)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    SerialNumber = "ADMIN001",
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@company.com",
                    PasswordHash = "TEMP_HASH", // Will be updated by initialization service
                    RoleId = 1, // Administrator
                    HireDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedDate = DateTime.UtcNow;
                }
            }
        }
    }
}