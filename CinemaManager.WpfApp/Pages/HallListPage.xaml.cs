using System.Windows.Controls;
using CinemaManager.Application.Dtos;
using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class HallListPage {
    private readonly HallListViewModel _vm;

    public HallListPage(HallListViewModel vm)
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

    private void HallListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (HallListBox.SelectedItem is not CinemaHallListItem hall) return;
        _vm.OnHallSelected(hall);
        HallListBox.SelectedItem = null;
    }
}
