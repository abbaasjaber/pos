using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NexusPOS.ViewModels
{
    public partial class MainViewModel : AppViewModelBase
    {
        [ObservableProperty]
        private AppViewModelBase _currentView;

        public POSViewModel PosViewModel { get; }
        public ProductViewModel ProductViewModel { get; }
        public DashboardViewModel DashboardViewModel { get; }

        public MainViewModel()
        {
            Title = "NexusPOS - Point of Sale";

            PosViewModel = new POSViewModel();
            ProductViewModel = new ProductViewModel();
            DashboardViewModel = new DashboardViewModel();

            CurrentView = DashboardViewModel; // Set initial view
        }

        [RelayCommand]
        private void NavigateTo(string viewName)
        {
            CurrentView = viewName switch
            {
                "POS" => PosViewModel,
                "Products" => ProductViewModel,
                _ => DashboardViewModel,
            };
        }
    }
}