# Company System - Training Project

## Project Overview
A comprehensive company management system built with ASP.NET Core, featuring role-based access control, employee management, department management, and content management capabilities.

## Technology Stack
- **Backend**: ASP.NET Core 9.0, C#, SQL Server, Entity Framework Core
- **Frontend**: HTML, CSS, JavaScript, MVC Pattern
- **Architecture**: 3-Layer Architecture (Web, Business, Data)

## Project Structure

### Solution Architecture
```
CompanySystem/
├── CompanySystem.Web/          # Presentation Layer (Controllers, Views, APIs)
├── CompanySystem.Business/     # Business Logic Layer (Services, Interfaces)
└── CompanySystem.Data/         # Data Access Layer (Models, DbContext, Repositories)
```

### 1. CompanySystem.Web (Presentation Layer)
```
CompanySystem.Web/
├── Areas/                      # Role-based areas
│   ├── Admin/                  # Administrator area
│   │   ├── Controllers/
│   │   └── Views/
│   ├── HR/                     # HR area
│   │   ├── Controllers/
│   │   └── Views/
│   ├── Lead/                   # Lead area
│   │   ├── Controllers/
│   │   └── Views/
│   └── Employee/               # Employee area
│       ├── Controllers/
│       └── Views/
├── Controllers/                # Main controllers
├── Views/                      # Main views
│   ├── Shared/                 # Shared layouts and partials
│   ├── Home/                   # Home page views
│   └── Account/                # Authentication views
├── ViewModels/                 # View models for data transfer
├── Helpers/                    # Helper classes and utilities
├── Middleware/                 # Custom middleware
└── wwwroot/                    # Static files
    ├── css/                    # Stylesheets
    ├── js/                     # JavaScript files
    ├── images/                 # Image assets
    └── lib/                    # Library files (Bootstrap, jQuery, etc.)
```

### 2. CompanySystem.Business (Business Logic Layer)
```
CompanySystem.Business/
├── Services/                   # Business services
│   ├── Auth/                   # Authentication services
│   ├── Department/             # Department management services
│   ├── User/                   # User management services
│   ├── Notes/                  # Notes management services
│   └── CMS/                    # Content management services
├── Interfaces/                 # Service interfaces
│   ├── Auth/                   # Authentication interfaces
│   ├── Department/             # Department interfaces
│   ├── User/                   # User interfaces
│   ├── Notes/                  # Notes interfaces
│   └── CMS/                    # CMS interfaces
├── DTOs/                       # Data Transfer Objects
├── Validators/                 # Validation classes
├── Exceptions/                 # Custom exceptions
├── Extensions/                 # Extension methods
└── Utilities/                  # Utility classes
```

### 3. CompanySystem.Data (Data Access Layer)
```
CompanySystem.Data/
├── Models/                     # Entity models
├── Data/                       # DbContext and configurations
├── Repositories/               # Repository pattern implementation
│   ├── Generic/                # Generic repository
│   └── Specific/               # Specific repositories
├── Configurations/             # Entity configurations (Fluent API)
├── Migrations/                 # Entity Framework migrations
├── SeedData/                   # Database seed data
└── Enums/                      # Enumerations
```

## Core Entities (Code First)
- **User**: Employee information, authentication, roles
- **Role**: User roles (Administrator, HR, Lead, Employee)
- **Department**: Department information and management
- **Notes**: Technical and behavioral notes
- **MainPageContent**: CMS content for main page

## Role-Based Access Control
1. **Administrator**: Full system access, user management, content management
2. **HR**: Employee profiles, salary management, behavioral notes
3. **Lead**: Team management, technical notes for subordinates
4. **Employee**: Self-service profile viewing

## Features
- ✅ User authentication and authorization
- ✅ Role-based access control
- ✅ Employee profile management
- ✅ Department management
- ✅ Notes system (Technical & Behavioral)
- ✅ Content management system
- ✅ Filtering and sorting capabilities
- ✅ Responsive design

## Development Phases
- **Week 1**: Foundation Setup (Authentication & Data Foundation)
- **Week 2**: Core Authentication & Departments
- **Week 3**: User Profiles & CMS
- **Week 4**: Notes System Foundation
- **Week 5**: Role-Specific Features
- **Week 6**: Advanced Features & Polish
- **Week 7**: Testing & Bug Fixes
- **Week 8**: Final Integration & Documentation

## Getting Started
1. Clone the repository
2. Open the solution in Visual Studio
3. Configure the connection string in `appsettings.json`
4. Run Entity Framework migrations
5. Seed the database with initial data
6. Build and run the application

## Database Setup
```bash
# Add migration
dotnet ef migrations add InitialCreate --project CompanySystem.Data --startup-project CompanySystem.Web

# Update database
dotnet ef database update --project CompanySystem.Data --startup-project CompanySystem.Web
```

## Project Dependencies
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.Extensions.DependencyInjection

## Contact
- **Email**: sayesh@asaltech.com
- **Mobile**: +970 598201254

---
*Training Project by Shady Ayesh - Asal Technologies* 