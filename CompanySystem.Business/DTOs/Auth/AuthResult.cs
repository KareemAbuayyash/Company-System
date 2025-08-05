using CompanySystem.Data.Entities;

namespace CompanySystem.Business.DTOs.Auth
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public User? User { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpiry { get; set; }
    }
} 