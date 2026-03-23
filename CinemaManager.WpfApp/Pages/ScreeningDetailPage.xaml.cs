using System.Windows;
using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class ScreeningDetailPage
{
    public ScreeningView Screening { get; }
    public string HallName { get; }

    public ScreeningDetailPage(ScreeningView screening, string hallName)
    {
        Screening = screening;
        HallName = hallName;
        InitializeComponent();
        DataContext = this;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e) =>
        NavigationService?.GoBack();
}
