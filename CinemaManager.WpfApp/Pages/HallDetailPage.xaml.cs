using System.Windows.Controls;
using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class HallDetailPage
{
    private readonly CinemaHallView _hall;

    public HallDetailPage(CinemaHallView hall)
    {
        InitializeComponent();
        _hall = hall;
        DataContext = hall;
    }

    private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e) =>
        NavigationService?.GoBack();

    private void ScreeningListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ScreeningListBox.SelectedItem is not ScreeningView screening) return;
        NavigationService?.Navigate(new ScreeningDetailPage(screening, _hall.Name));
        ScreeningListBox.SelectedItem = null;
    }
}
