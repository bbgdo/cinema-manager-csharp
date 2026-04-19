using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class HallEditPage
{
    private readonly HallEditViewModel _vm;

    public HallEditPage(HallEditViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = vm;
        Loaded += async (_, _) =>
        {
            if (_vm.IsBusy) return;
            await _vm.LoadAsync();
        };
    }
}
