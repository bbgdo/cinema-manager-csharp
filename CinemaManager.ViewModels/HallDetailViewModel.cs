using System.Windows.Input;
using CinemaManager.Application.Dtos;

namespace CinemaManager.ViewModels;

public class HallDetailViewModel
{
    private readonly INavigationService _navigation;

    public CinemaHallDetail Hall { get; }
    public ICommand BackCommand { get; }

    public HallDetailViewModel(CinemaHallDetail hall, INavigationService navigation)
    {
        Hall = hall;
        _navigation = navigation;
        BackCommand = new RelayCommand(() => _navigation.GoBack());
    }

    public void OnScreeningSelected(ScreeningListItem screening) =>
        _navigation.GoToScreeningDetail(screening.Id, Hall.Name);
}
