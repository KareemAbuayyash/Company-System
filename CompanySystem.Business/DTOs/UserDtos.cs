using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string FullName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateUserDto
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public int RoleId { get; set; }

        public int? DepartmentId { get; set; }
    }

    public class EditUserDto
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int RoleId { get; set; }

        public int? DepartmentId { get; set; }
    }

    public class UserSearchResultDto
    {
        public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();
        public int TotalCount { get; set; }
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "name";
        public bool HasSearch { get; set; }
    }
}
