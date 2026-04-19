using System.Windows.Controls;
using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class ScreeningDetailPage : Page
{
    private readonly ScreeningDetailViewModel _vm;
    private bool _loaded;

    public ScreeningDetailPage(ScreeningDetailViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = vm;
        Loaded += async (_, _) =>
        {
            if (_loaded) return;
            _loaded = true;
            await _vm.LoadAsync();
        };
    }
}
