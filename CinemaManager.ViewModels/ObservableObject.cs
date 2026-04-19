using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CinemaManager.ViewModels;

public class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsBusy { get; private set; }

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        protected set
        {
            if (_errorMessage == value) return;
            _errorMessage = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected IDisposable EnterBusy()
    {
        IsBusy = true;
        OnPropertyChanged(nameof(IsBusy));
        return new BusyScope(this);
    }

    protected async Task RunAsync(Func<Task> operation)
    {
        ErrorMessage = null;
        using (EnterBusy())
        {
            try { await operation(); }
            catch (Exception ex) { ErrorMessage = ex.Message; }
        }
    }

    private sealed class BusyScope(ObservableObject owner) : IDisposable
    {
        public void Dispose()
        {
            owner.IsBusy = false;
            owner.OnPropertyChanged(nameof(IsBusy));
        }
    }
}
