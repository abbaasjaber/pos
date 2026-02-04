using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NexusPOS.Models
{
    public class ProductGroup : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        // Navigation
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
