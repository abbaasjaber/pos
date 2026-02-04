using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexusPOS.Data.Context;
using NexusPOS.Models;
using NexusPOS.Services;
using System;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace NexusPOS.ViewModels
{
    public partial class POSViewModel : AppViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<InvoiceItem> _cartItems = new();

        [ObservableProperty]
        private decimal _totalAmount;

        [ObservableProperty]
        private decimal _discount;

        [ObservableProperty]
        private decimal _finalPayable;

        [ObservableProperty]
        private string _barcodeInput = "";

        public POSViewModel()
        {
            CartItems.CollectionChanged += CartItems_CollectionChanged;
            Recalculate();
        }

        private void CartItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (InvoiceItem item in e.NewItems)
                    item.PropertyChanged += Item_PropertyChanged;
            }

            if (e.OldItems != null)
            {
                foreach (InvoiceItem item in e.OldItems)
                    item.PropertyChanged -= Item_PropertyChanged;
            }
            Recalculate();
        }

        private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InvoiceItem.TotalPrice)) Recalculate();
        }

        [RelayCommand]
        private void AddToCart()
        {
            // Create a dummy product
            var dummyProduct = new Product 
            { 
                Code = "P" + (CartItems.Count + 1),
                Name = "کالای نمونه " + (CartItems.Count + 1), 
                SellPrice = 150000 
            };

            var item = new InvoiceItem
            {
                ProductId = 0, // Dummy product has no ID
                Product = dummyProduct,
                UnitPrice = dummyProduct.SellPrice,
                Quantity = 1, // Setting Quantity calculates TotalPrice
            };
            CartItems.Add(item);
        }

        [RelayCommand]
        private void Pay()
        {
            if (CartItems.Count == 0) return;

            // Create Invoice object for printing
            var invoice = new Invoice
            {
                InvoiceNo = DateTime.Now.ToString("yyyyMMddHHmm"),
                Date = DateTime.Now,
                TotalAmount = TotalAmount,
                Discount = Discount,
                FinalAmount = FinalPayable
            };

            var printService = new PrintService();
            printService.PrintInvoice(invoice, CartItems);

            MessageBox.Show($"فاکتور با مبلغ {FinalPayable:N0} ریال صادر شد.", "موفقیت");
            CartItems.Clear();
            Recalculate();
        }

        private void Recalculate()
        {
            TotalAmount = CartItems.Sum(i => i.TotalPrice);
            FinalPayable = TotalAmount - Discount;
        }

        public void ProcessBarcode(string barcode)
        {
            using var context = new AppDbContext();
            // Find product by Barcode or Code
            var product = context.Products.FirstOrDefault(p => p.Barcode == barcode || p.Code == barcode);

            if (product != null)
            {
                var existingItem = CartItems.FirstOrDefault(i => i.Product?.Id == product.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    var newItem = new InvoiceItem
                    {
                        ProductId = product.Id,
                        Product = product,
                        UnitPrice = product.SellPrice,
                        Quantity = 1 // This will auto-calculate TotalPrice
                    };
                    CartItems.Add(newItem);
                }
                // Recalculate is now triggered automatically by events
            }
            else
            {
                // Optional: Beep or show small notification
                System.Media.SystemSounds.Beep.Play();
            }
        }

        [RelayCommand]
        private void SubmitBarcode()
        {
            if (!string.IsNullOrWhiteSpace(BarcodeInput))
            {
                ProcessBarcode(BarcodeInput);
                BarcodeInput = ""; // Clear input for next scan
            }
        }
    }
}
