namespace CompanySystem.Data.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        int? CreatedById { get; set; }
        int? UpdatedById { get; set; }
        bool IsDeleted { get; set; }
    }
} 