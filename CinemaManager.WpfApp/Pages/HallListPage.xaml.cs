using System.Windows.Controls;
using CinemaManager.Services;
using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class HallListPage
{
    private readonly ICinemaRepository _repository;

    public HallListPage(ICinemaRepository repository)
    {
        InitializeComponent();
        _repository = repository;
        HallListBox.ItemsSource = _repository.GetAllHallViews();
    }

    private void HallListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (HallListBox.SelectedItem is not CinemaHallView hall) return;
        _repository.LoadScreeningsForHall(hall);
        NavigationService?.Navigate(new HallDetailPage(hall));
        HallListBox.SelectedItem = null;
    }
}