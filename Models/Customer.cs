using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NexusPOS.Models
{
    public class Customer : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
