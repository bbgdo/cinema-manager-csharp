using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CinemaManager.ViewModels;

public class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsBusy { get; private set; }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void BeginBusy()
    {
        IsBusy = true;
        OnPropertyChanged(nameof(IsBusy));
    }

    protected void EndBusy()
    {
        IsBusy = false;
        OnPropertyChanged(nameof(IsBusy));
    }
}
