using CommunityToolkit.Mvvm.ComponentModel;

namespace NexusPOS.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string? _title;
    }
}