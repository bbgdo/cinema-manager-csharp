using CinemaManager.Application;
using CinemaManager.Application.Dtos;

namespace CinemaManager.ViewModels;

public class HallListViewModel
{
    private readonly INavigationService _navigation;

    public IReadOnlyList<CinemaHallListItem> Halls { get; }

    public HallListViewModel(ICinemaService cinemaService, INavigationService navigation)
    {
        _navigation = navigation;
        Halls = cinemaService.GetHallList();
    }

    public void OnHallSelected(CinemaHallListItem hall) =>
        _navigation.GoToHallDetail(hall.Id);
}
