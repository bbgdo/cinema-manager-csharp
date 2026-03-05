using System.Windows;
using System.Windows.Controls;
using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class ScreeningDetailPage
{
    public ScreeningDetailPage(ScreeningView screening, string hallName)
    {
        InitializeComponent();

        var fields = new[]
        {
            ("Movie",    screening.MovieTitle),
            ("Hall",     hallName),
            ("Genre",    screening.Genre.ToDisplayName()),
            ("Year",     screening.ReleaseYear.ToString()),
            ("Start",    screening.StartTime.ToString("dd.MM.yyyy HH:mm")),
            ("End",      screening.EndTime.ToString("dd.MM.yyyy HH:mm")),
            ("Duration", $"{screening.DurationMinutes} min"),
        };

        foreach (var (label, value) in fields)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 8) };
            row.Children.Add(new TextBlock { Text = $"{label}:", Width = 80, FontWeight = FontWeights.SemiBold });
            row.Children.Add(new TextBlock { Text = value });
            DetailsPanel.Children.Add(row);
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e) =>
        NavigationService?.GoBack();
}