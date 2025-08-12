// CompanySystem.Data/Models/BaseEntity.cs
using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Data.Models
{
    public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; } = default!;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        public string? UpdatedBy { get; set; }
    }

    // Convenience class for entities with int primary key
    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
