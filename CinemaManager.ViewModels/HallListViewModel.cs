using CinemaManager.Application;
using CinemaManager.Application.Dtos;

namespace CinemaManager.ViewModels;

public class HallListViewModel(ICinemaService cinemaService, INavigationService navigation) : ObservableObject
{
    private IReadOnlyList<CinemaHallListItem>? _halls;

    public IReadOnlyList<CinemaHallListItem>? Halls
    {
        get => _halls;
        private set => SetProperty(ref _halls, value);
    }

    public RelayCommand AddHallCommand { get; } = new RelayCommand(() => navigation.GoToHallEdit(null));

    public async Task LoadAsync() =>
        await RunAsync(async () => Halls = await cinemaService.GetHallListAsync());

    public void OnHallSelected(CinemaHallListItem hall) =>
        navigation.GoToHallDetail(hall.Id);
}
