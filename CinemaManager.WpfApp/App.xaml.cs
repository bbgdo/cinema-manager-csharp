using CinemaManager.Application;
using CinemaManager.Services;
using CinemaManager.ViewModels;
using CinemaManager.Wpf.Dialogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace CinemaManager.Wpf;

public partial class App
{
    public static IServiceProvider Services { get; private set; } = null!;

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        var dbDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CinemaManager");
        Directory.CreateDirectory(dbDir);
        var dbPath = Path.Combine(dbDir, "cinema.db");

        var services = new ServiceCollection();
        services.AddDbContextFactory<CinemaDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));
        services.AddSingleton<ICinemaRepository, CinemaRepository>();
        services.AddSingleton<ICinemaService, CinemaService>();
        services.AddSingleton<IDialogService, DialogService>();
        Services = services.BuildServiceProvider();

        var factory = Services.GetRequiredService<IDbContextFactory<CinemaDbContext>>();
        await DatabaseInitializer.InitializeAsync(factory);

        new MainWindow().Show();
    }
}
