using CinemaManager.Application;
using CinemaManager.Services;
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
        services.AddSingleton<ICinemaService, CinemaService>();
        Services = services.BuildServiceProvider();

        new MainWindow().Show();
    }
}
