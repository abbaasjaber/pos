using System;
using System.ComponentModel.DataAnnotations;

namespace NexusPOS.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public enum UserRole
    {
        Admin,
        Sales,
        Accountant
    }
}
