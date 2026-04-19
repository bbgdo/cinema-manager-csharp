using System.Windows.Input;
using CinemaManager.Application;
using CinemaManager.Application.Dtos;

namespace CinemaManager.ViewModels;

public class HallDetailViewModel : ObservableObject
{
    private readonly int _hallId;
    private readonly ICinemaService _cinemaService;
    private readonly INavigationService _navigation;
    private CinemaHallDetail? _hall;

    public CinemaHallDetail? Hall
    {
        get => _hall;
        private set => SetProperty(ref _hall, value);
    }

    public ICommand BackCommand { get; }

    public HallDetailViewModel(int hallId, ICinemaService cinemaService, INavigationService navigation)
    {
        _hallId = hallId;
        _cinemaService = cinemaService;
        _navigation = navigation;
        BackCommand = new RelayCommand(() => _navigation.GoBack());
    }

    public async Task LoadAsync()
    {
        BeginBusy();
        try
        {
            Hall = await _cinemaService.GetHallDetailAsync(_hallId);
        }
        finally
        {
            EndBusy();
        }
    }

    public void OnScreeningSelected(ScreeningListItem screening) =>
        _navigation.GoToScreeningDetail(screening.Id, Hall?.Name ?? string.Empty);
}
