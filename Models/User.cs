using System.ComponentModel.DataAnnotations;

namespace NexusPOS.Models
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }

        [Required]
        public required string PasswordHash { get; set; } // In production, use BCrypt/Argon2

        public UserRole Role { get; set; } = UserRole.Sales;
        
        [MaxLength(100)]
        public string? FullName { get; set; }
    }
}
