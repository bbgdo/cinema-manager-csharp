using System.Windows.Controls;
using CinemaManager.Application.Dtos;
using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class HallDetailPage {
    private readonly HallDetailViewModel _vm;
    private bool _loaded;

    public HallDetailPage(HallDetailViewModel vm)
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

    private void ScreeningListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ScreeningListBox.SelectedItem is not ScreeningListItem screening) return;
        _vm.OnScreeningSelected(screening);
        ScreeningListBox.SelectedItem = null;
    }
}
