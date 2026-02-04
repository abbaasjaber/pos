using CommunityToolkit.Mvvm.ComponentModel;

namespace NexusPOS.ViewModels
{
    // The single source of truth for the base view model
    public partial class AppViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string? _title;
    }
}