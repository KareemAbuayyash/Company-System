namespace CompanySystem.Business.Interfaces.Auth
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
} 