# CompanySystem.Data - Authentication Foundation

## AHMAD ALARJAH - WEEK 1 AUTHENTICATION FOUNDATION

This Data layer contains the core entities, interfaces, and seed data for the authentication system.

## Project Structure

```
CompanySystem.Data/
├── Models/
│   ├── User.cs (✅ Complete with Data Annotations)
│   └── Role.cs (✅ Complete with Data Annotations)
├── Interfaces/
│   ├── IAuditableEntity.cs (✅ Base interface for audit fields)
│   ├── IRepository.cs (✅ Generic repository pattern)
│   ├── IUnitOfWork.cs (✅ Transaction management)
│   ├── IAuthenticationService.cs (✅ Authentication contract)
│   ├── IUserRepository.cs (✅ User-specific operations)
│   └── IRoleRepository.cs (✅ Role-specific operations)
└── SeedData/
    └── InitialRoles.cs (✅ Initial role data)
```

## Features Implemented

### Entities (Models)
- **User Entity**: Complete user model with all required fields, Data Annotations, navigation properties, and audit fields
- **Role Entity**: Role model with navigation to users, proper constraints, and audit fields using Data Annotations
- **IAuditableEntity**: Base interface ensuring all entities have audit fields (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, IsDeleted)

### Interfaces
- **IRepository<T>**: Generic repository pattern for CRUD operations with soft deletion support
- **IUnitOfWork**: Transaction management interface
- **IAuthenticationService**: Authentication service contract
- **IUserRepository**: User-specific repository operations with soft deletion methods
- **IRoleRepository**: Role-specific repository operations
- **IAuditableEntity**: Base interface for entities with audit fields

### Data Annotations & Seed Data
- **User Entity**: Configured with Data Annotations for validation, constraints, and relationships
- **Role Entity**: Configured with Data Annotations for validation, constraints, and relationships
- **InitialRoles**: Static class providing seed data for initial roles (Administrator, HR, Lead, Employee)

## Initial Roles Seeded
1. Administrator
2. HR
3. Lead
4. Employee

## Audit Fields & Soft Deletion

### Audit Fields
All entities implement `IAuditableEntity` with the following audit fields:
- **CreatedAt**: Automatically set to UTC timestamp when entity is created
- **UpdatedAt**: Set to UTC timestamp when entity is modified
- **CreatedBy**: Username/ID of who created the entity
- **UpdatedBy**: Username/ID of who last modified the entity
- **IsDeleted**: Soft deletion flag (default: false)

### Soft Deletion Support
- **SoftDeleteAsync()**: Marks entity as deleted without removing from database
- **RestoreAsync()**: Restores soft-deleted entity
- **GetAllIncludingDeletedAsync()**: Retrieves all entities including soft-deleted ones
- **GetDeletedUsersAsync()**: Retrieves only soft-deleted users
- **GetByEmailIncludingDeletedAsync()**: Finds user by email including soft-deleted

## Ready for Implementation

The Data layer is now complete and ready for:
- **Business Layer**: Implement IAuthenticationService
- **Data Layer**: Implement DbContext using Data Annotations
- **Data Layer**: Implement repositories using interfaces
- **Web Layer**: Create authentication controllers

## Dependencies
- Microsoft.EntityFrameworkCore.SqlServer (9.0.7)
- Microsoft.EntityFrameworkCore.Tools (9.0.7)
- System.ComponentModel.Annotations (5.0.0)

---

**AHMAD'S AUTHENTICATION FOUNDATION COMPLETE! 🚀** 