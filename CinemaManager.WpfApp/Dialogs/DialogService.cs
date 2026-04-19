using System.Windows;
using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Dialogs;

public class DialogService : IDialogService
{
    public Task<bool> ConfirmAsync(string title, string message) =>
        Task.FromResult(
            MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question)
            == MessageBoxResult.Yes);
}
