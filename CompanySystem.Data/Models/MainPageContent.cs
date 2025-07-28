using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Data.Models
{
    public class MainPageContent : BaseEntity
    {
        [Key]
        public int ContentId { get; set; }

        [Required]
        [StringLength(50)]
        public string SectionName { get; set; }  // Overview, AboutUs, Services

        [Required]
        [StringLength(200)]
        public string Title { get; set; } 

        [Required]
        public string Content { get; set; } 
    }
}
