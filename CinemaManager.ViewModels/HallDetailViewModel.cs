using System.Windows.Input;
using CinemaManager.Application;
using CinemaManager.Application.Dtos;

namespace CinemaManager.ViewModels;

public class HallDetailViewModel : ObservableObject
{
    private readonly int _hallId;
    private readonly ICinemaService _cinemaService;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogService;
    private CinemaHallDetail? _hall;
    private readonly RelayCommand _addScreeningCommand;

    public CinemaHallDetail? Hall
    {
        get => _hall;
        private set
        {
            if (SetProperty(ref _hall, value))
                _addScreeningCommand.RaiseCanExecuteChanged();
        }
    }

    public ICommand BackCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand AddScreeningCommand => _addScreeningCommand;

    public HallDetailViewModel(int hallId, ICinemaService cinemaService, INavigationService navigation, IDialogService dialogService)
    {
        _hallId = hallId;
        _cinemaService = cinemaService;
        _navigation = navigation;
        _dialogService = dialogService;
        BackCommand = new RelayCommand(() => _navigation.GoBack());
        EditCommand = new RelayCommand(() => _navigation.GoToHallEdit(_hallId));
        DeleteCommand = new AsyncRelayCommand(DeleteAsync);
        _addScreeningCommand = new RelayCommand(
            () => _navigation.GoToScreeningEdit(null, _hallId, Hall?.Name ?? string.Empty),
            () => Hall is not null);
    }

    public async Task LoadAsync() =>
        await RunAsync(async () => Hall = await _cinemaService.GetHallDetailAsync(_hallId));

    public void OnScreeningSelected(ScreeningListItem screening) =>
        _navigation.GoToScreeningDetail(screening.Id, _hallId, Hall?.Name ?? string.Empty);

    private async Task DeleteAsync()
    {
        var confirmed = await _dialogService.ConfirmAsync(
            "Delete Hall",
            $"Delete '{Hall?.Name ?? "this hall"}'? All screenings in this hall will be removed.");
        if (!confirmed) return;
        await RunAsync(async () =>
        {
            await _cinemaService.DeleteHallAsync(_hallId);
            _navigation.GoBack();
        });
    }
}
