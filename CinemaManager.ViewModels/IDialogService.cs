namespace CinemaManager.ViewModels;

public interface IDialogService
{
    Task<bool> ConfirmAsync(string title, string message);
}
