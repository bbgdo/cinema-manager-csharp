using System.Windows.Input;
using CinemaManager.Application;
using CinemaManager.Application.Dtos;

namespace CinemaManager.ViewModels;

public class ScreeningDetailViewModel : ObservableObject
{
    private readonly int _screeningId;
    private readonly ICinemaService _cinemaService;
    private readonly string _hallName;
    private ScreeningDetail? _screening;

    public ScreeningDetail? Screening
    {
        get => _screening;
        private set => SetProperty(ref _screening, value);
    }

    public string HallName => _hallName;
    public ICommand BackCommand { get; }

    public ScreeningDetailViewModel(int screeningId, string hallName, ICinemaService cinemaService, INavigationService navigation)
    {
        _screeningId = screeningId;
        _hallName = hallName;
        _cinemaService = cinemaService;
        BackCommand = new RelayCommand(() => navigation.GoBack());
    }

    public async Task LoadAsync()
    {
        BeginBusy();
        try
        {
            Screening = await _cinemaService.GetScreeningDetailAsync(_screeningId);
        }
        finally
        {
            EndBusy();
        }
    }
}
