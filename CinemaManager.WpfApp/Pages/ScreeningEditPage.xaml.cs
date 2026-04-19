using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class ScreeningEditPage
{
    private readonly ScreeningEditViewModel _vm;

    public ScreeningEditPage(ScreeningEditViewModel vm)
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
