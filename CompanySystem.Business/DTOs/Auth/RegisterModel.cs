namespace CompanySystem.Business.DTOs.Auth
{
    public class RegisterModel
    {
        public string SerialNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime HireDate { get; set; } = DateTime.UtcNow;
        public decimal? Salary { get; set; }
        public string? Skills { get; set; }
        public string? Experience { get; set; }
    }
} 