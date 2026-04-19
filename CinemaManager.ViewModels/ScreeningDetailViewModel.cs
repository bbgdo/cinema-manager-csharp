using System.Windows.Input;
using CinemaManager.Application;
using CinemaManager.Application.Dtos;

namespace CinemaManager.ViewModels;

public class ScreeningDetailViewModel : ObservableObject
{
    private readonly int _screeningId;
    private readonly int _hallId;
    private readonly ICinemaService _cinemaService;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogService;
    private readonly string _hallName;
    private ScreeningDetail? _screening;
    private readonly RelayCommand _editCommand;
    private readonly AsyncRelayCommand _deleteCommand;

    public ScreeningDetail? Screening
    {
        get => _screening;
        private set
        {
            if (SetProperty(ref _screening, value))
            {
                _editCommand.RaiseCanExecuteChanged();
                _deleteCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string HallName => _hallName;
    public ICommand BackCommand { get; }
    public ICommand EditCommand => _editCommand;
    public ICommand DeleteCommand => _deleteCommand;

    public ScreeningDetailViewModel(int screeningId, int hallId, string hallName, ICinemaService cinemaService, INavigationService navigation, IDialogService dialogService)
    {
        _screeningId = screeningId;
        _hallId = hallId;
        _hallName = hallName;
        _cinemaService = cinemaService;
        _navigation = navigation;
        _dialogService = dialogService;
        BackCommand = new RelayCommand(() => navigation.GoBack());
        _editCommand = new RelayCommand(
            () => _navigation.GoToScreeningEdit(_screeningId, _hallId, _hallName),
            () => Screening is not null);
        _deleteCommand = new AsyncRelayCommand(DeleteAsync, () => Screening is not null);
    }

    public async Task LoadAsync() =>
        await RunAsync(async () => Screening = await _cinemaService.GetScreeningDetailAsync(_screeningId));

    private async Task DeleteAsync()
    {
        var confirmed = await _dialogService.ConfirmAsync(
            "Delete Screening",
            $"Delete '{Screening?.MovieTitle ?? "this screening"}'?");
        if (!confirmed) return;
        await RunAsync(async () =>
        {
            await _cinemaService.DeleteScreeningAsync(_screeningId);
            _navigation.GoBack();
        });
    }
}
