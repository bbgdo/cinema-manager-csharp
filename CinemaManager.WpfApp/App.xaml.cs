using CinemaManager.Services;
using CinemaManager.Wpf.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CinemaManager.Wpf;

public partial class App
{
    public static IServiceProvider Services { get; private set; } = null!;

    private void OnStartup(object sender, StartupEventArgs e)
    {
        var services = new ServiceCollection();
        services.AddSingleton<ICinemaRepository, CinemaRepository>();
        services.AddTransient<HallListPage>();
        services.AddTransient<HallDetailPage>();
        services.AddTransient<ScreeningDetailPage>();
        Services = services.BuildServiceProvider();

        new MainWindow().Show();
    }
}