using System.Windows.Controls;
using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class ScreeningDetailPage : Page
{
    private readonly ScreeningDetailViewModel _vm;

    public ScreeningDetailPage(ScreeningDetailViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = vm;
        Loaded += async (_, _) => await _vm.LoadAsync();
    }
}
