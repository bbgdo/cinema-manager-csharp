using CinemaManager.ViewModels;

namespace CinemaManager.Services;

public interface ICinemaRepository
{
    List<CinemaHallView> GetAllHallViews();
    void LoadScreeningsForHall(CinemaHallView hallView);
}