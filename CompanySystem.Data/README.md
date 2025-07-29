# Company System Data Layer

## Repository Pattern Implementation

### Overview
This project implements a clean, generic repository pattern with proper inheritance and minimal code duplication.

### Structure

```
CompanySystem.Data/
├── Interfaces/
│   ├── IBaseRepository.cs          # Generic base interface
│   ├── IUserRepository.cs          # User-specific interface (minimal)
│   ├── IRoleRepository.cs          # Role-specific interface (minimal)
│   └── IAuditableEntity.cs         # Audit interface
├── Repositories/
│   ├── BaseRepository.cs           # Generic base implementation
│   ├── UserRepository.cs           # User-specific implementation
│   └── RoleRepository.cs           # Role-specific implementation
├── Models/
│   ├── User.cs                     # User entity
│   └── Role.cs                     # Role entity
└── Data/
    └── CompanyDbContext.cs         # DbContext
```

### Available Methods

#### Generic Methods (Available on all repositories)
```csharp
// Basic CRUD
await repo.GetByIdAsync<int>(id);
await repo.GetAllAsync();
await repo.AddAsync(entity);
await repo.AddRangeAsync(entities);
await repo.UpdateAsync(entity);
await repo.DeleteAsync(entity);
await repo.DeleteByIdAsync<int>(id);

// Query operations
await repo.FindAsync(predicate);
await repo.FirstOrDefaultAsync(predicate);
await repo.ExistsAsync(predicate);
await repo.CountAsync(predicate);

// Soft delete operations
await repo.SoftDeleteAsync(entity);
await repo.SoftDeleteByIdAsync<int>(id);
await repo.RestoreAsync(entity);
await repo.RestoreByIdAsync<int>(id);
```

#### Specific Methods (Only where needed)
```csharp
// UserRepository
await userRepo.GetByEmailAsync(email);
await userRepo.GetByEmployeeIdAsync(employeeId);

// RoleRepository
await roleRepo.GetByNameAsync(roleName);
```

### Usage Examples

#### Instead of specific methods, use predicates:
```csharp
// ❌ Don't create specific methods for these:
// GetByRoleAsync(int roleId)
// GetActiveUsersAsync()
// GetDeletedUsersAsync()

// ✅ Use existing generic methods with predicates:
await userRepo.FindAsync(u => u.RoleId == roleId && !u.IsDeleted);
await userRepo.FindAsync(u => u.IsActive && !u.IsDeleted);
await userRepo.FindAsync(u => u.IsDeleted);

// ✅ For single results:
await userRepo.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
await userRepo.FirstOrDefaultAsync(u => u.EmployeeId == employeeId && !u.IsDeleted);
```

#### Including deleted entities:
```csharp
// Get all users including deleted
await userRepo.FindAsync(u => true);

// Get user by email including deleted
await userRepo.FirstOrDefaultAsync(u => u.Email == email);
```

#### Complex queries:
```csharp
// Get users by multiple criteria
await userRepo.FindAsync(u => 
    u.RoleId == roleId && 
    u.IsActive && 
    u.Salary > 50000 && 
    !u.IsDeleted);

// Count active users
await userRepo.CountAsync(u => u.IsActive && !u.IsDeleted);

// Check if email exists
await userRepo.ExistsAsync(u => u.Email == email && !u.IsDeleted);
```

### Key Benefits

1. **DRY Principle**: No code duplication
2. **Type Safety**: Generic methods with proper constraints
3. **Flexibility**: Use predicates for any query
4. **Maintainability**: Easy to add new entities
5. **Consistency**: All repositories follow the same pattern
6. **Automatic Audit**: Audit fields are set automatically
7. **Smart Filtering**: Deleted entities are excluded by default

### Best Practices

1. **Use predicates instead of specific methods** for common queries
2. **Only create specific methods** when they require special logic
3. **Leverage the generic methods** for most operations
4. **Use the audit functionality** for soft deletes and tracking
5. **Keep repositories focused** on data access only 