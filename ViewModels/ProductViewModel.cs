using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexusPOS.Data.Context;
using NexusPOS.Models;
using NexusPOS.Views.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System;

namespace NexusPOS.ViewModels
{
    public partial class ProductViewModel : AppViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<Product> _products = new();

        [ObservableProperty]
        private ObservableCollection<Product> _filteredProducts = new();

        [ObservableProperty]
        private string _searchText = "";

        [ObservableProperty]
        private Product _newProduct = new() { Code = "", Name = "" };

        public ProductViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            using var context = new AppDbContext();
            var list = context.Products.OrderByDescending(p => p.Id).ToList();
            Products = new ObservableCollection<Product>(list);
            
            // Refresh filter
            OnSearchTextChanged(SearchText);
        }

        [RelayCommand]
        private void OpenAddProduct()
        {
            // Initialize new product with required fields
            NewProduct = new Product 
            {
                Code = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(), // Auto-generate unique code
                Name = "",
                GroupId = 1 // Default to 'General' group seeded in AppDbContext
            };

            var window = new AddProductWindow();
            window.DataContext = this;
            window.ShowDialog();
        }

        [RelayCommand]
        private void SaveProduct(Window window)
        {
            if (string.IsNullOrWhiteSpace(NewProduct?.Name))
            {
                MessageBox.Show("لطفا نام کالا را وارد کنید.", "خطا");
                return;
            }

            using var context = new AppDbContext();
            context.Products.Add(NewProduct);
            context.SaveChanges();

            LoadData();
            window?.Close();
        }

        partial void OnSearchTextChanged(string value)
        {
            FilteredProducts.Clear();
            IEnumerable<Product> productsToShow;

            if (string.IsNullOrWhiteSpace(value))
            {
                productsToShow = Products;
            }
            else
            {
                productsToShow = Products.Where(p => p.Name.Contains(value, StringComparison.OrdinalIgnoreCase));
            }
            foreach (var product in productsToShow) FilteredProducts.Add(product);
        }
    }
}