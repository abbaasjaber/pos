using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusPOS.Models
{
    public class Invoice : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public required string InvoiceNo { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalAmount { get; set; }

        public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}
