using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusPOS.Models
{
    public partial class InvoiceItem : ObservableObject
    {
        public int Id { get; set; }

        // Foreign keys for DB
        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (SetProperty(ref _quantity, value))
                {
                    // When quantity changes, update TotalPrice
                    TotalPrice = value * UnitPrice;
                }
            }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (SetProperty(ref _unitPrice, value))
                {
                    // When unit price changes, update TotalPrice
                    TotalPrice = Quantity * value;
                }
            }
        }

        [ObservableProperty]
        private decimal _totalPrice;
    }
}