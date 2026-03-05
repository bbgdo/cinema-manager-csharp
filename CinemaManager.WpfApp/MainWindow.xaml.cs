using System.Windows;
using CinemaManager.Wpf.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaManager.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        RootFrame.Navigate(App.Services.GetRequiredService<HallListPage>());
    }
}