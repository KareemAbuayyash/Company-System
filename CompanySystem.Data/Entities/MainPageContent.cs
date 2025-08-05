using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CompanySystem.Data.Enums;

namespace CompanySystem.Data.Entities
{
    [Table("MainPageContent")]
    public class MainPageContent : BaseEntity
    {
        [Required]
        public SectionName SectionName { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; } = string.Empty;

        // Foreign Key
        [Required]
        [ForeignKey("UpdatedByUser")]
        public int UpdatedById { get; set; }




    }
} 