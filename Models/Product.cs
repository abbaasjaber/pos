using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusPOS.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public required string Code { get; set; } // Required for Search/POS

        [MaxLength(50)]
        public string? Barcode { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        public string? ImageUrl { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BuyPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SellPrice { get; set; } // Maps to "Price" in your request

        public int Stock { get; set; }
        public int MinStock { get; set; }

        // Foreign Key
        public int GroupId { get; set; }
        public virtual ProductGroup? Group { get; set; }
    }
}
