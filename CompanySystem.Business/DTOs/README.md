# DTOs (Data Transfer Objects)

This folder contains all Data Transfer Objects used throughout the application. DTOs are organized by domain/feature to maintain clear separation of concerns.

## Structure

```
DTOs/
├── Auth/                    # Authentication and authorization DTOs
│   ├── AuthResult.cs       # Result object for authentication operations
│   ├── LoginModel.cs       # Login request model
│   ├── RegisterModel.cs    # User registration model
│   ├── ChangePasswordModel.cs  # Password change request model
│   ├── ResetPasswordModel.cs   # Password reset request model
│   └── SetPasswordModel.cs     # Set new password model
└── README.md              # This documentation file
```

## Guidelines

1. **Separation by Domain**: DTOs are organized by business domain (Auth, User, Department, etc.)
2. **Single Responsibility**: Each DTO should have a single, well-defined purpose
3. **Naming Convention**: Use descriptive names ending with "Model" for input DTOs and "Result" for output DTOs
4. **Validation**: DTOs should include basic validation attributes where appropriate
5. **Documentation**: Each DTO should be well-documented with XML comments

## Usage

DTOs are used to:
- Transfer data between layers (Web → Business → Data)
- Define API contracts
- Separate internal models from external representations
- Provide type safety for data transfer

## Adding New DTOs

When adding new DTOs:
1. Create the appropriate domain folder if it doesn't exist
2. Add the DTO file with proper naming convention
3. Update this README with the new DTO
4. Add XML documentation to the DTO class and properties 