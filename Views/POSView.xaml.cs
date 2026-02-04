using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NexusPOS.ViewModels;

namespace NexusPOS.Views
{
    public partial class POSView : UserControl
    {
        private string _barcodeBuffer = "";

        public POSView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.PreviewKeyDown += Window_PreviewKeyDown;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.PreviewKeyDown -= Window_PreviewKeyDown;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // If focus is on a TextBox, ignore global scanner listener to allow manual typing
            if (Keyboard.FocusedElement is TextBox) return;

            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(_barcodeBuffer))
                {
                    if (DataContext is POSViewModel vm)
                    {
                        vm.ProcessBarcode(_barcodeBuffer);
                    }
                    _barcodeBuffer = "";
                    e.Handled = true; // Prevent default Enter behavior
                }
                return;
            }

            // Capture Numbers (Mainly for Barcodes)
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
                _barcodeBuffer += (e.Key - Key.D0).ToString();
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                _barcodeBuffer += (e.Key - Key.NumPad0).ToString();
            // Capture Letters (if barcode contains them)
            else if (e.Key >= Key.A && e.Key <= Key.Z)
                _barcodeBuffer += e.Key.ToString();
        }
    }
}