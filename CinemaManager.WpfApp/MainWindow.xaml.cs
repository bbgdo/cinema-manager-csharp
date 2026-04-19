using System.Windows;
using CinemaManager.Application;
using CinemaManager.ViewModels;
using CinemaManager.Wpf.Navigation;
using CinemaManager.Wpf.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaManager.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var cinemaService = App.Services.GetRequiredService<ICinemaService>();
        var dialogService = App.Services.GetRequiredService<IDialogService>();
        var navigation = new FrameNavigationService(RootFrame, cinemaService, dialogService);
        var vm = new HallListViewModel(cinemaService, navigation);

        RootFrame.Navigate(new HallListPage(vm));
    }
}
