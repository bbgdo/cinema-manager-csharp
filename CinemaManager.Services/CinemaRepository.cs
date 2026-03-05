using CinemaManager.Models;
using CinemaManager.ViewModels;

namespace CinemaManager.Services;

public class CinemaRepository : ICinemaRepository {
    public IReadOnlyList<CinemaHall> GetAllHalls() =>
        FakeDataStorage.CinemaHalls;

    public IReadOnlyList<Screening> GetScreeningsByHall(int hallId) =>
        FakeDataStorage.Screenings.Where(s => s.HallId == hallId).ToList();

    public List<CinemaHallView> GetAllHallViews() =>
        GetAllHalls().Select(h => new CinemaHallView(h)).ToList();

    public void LoadScreeningsForHall(CinemaHallView hallView)
    {
        if (hallView.ScreeningsLoaded) return;

        var screeningViews = GetScreeningsByHall(hallView.Id)
            .Select(s => new ScreeningView(s));

        hallView.LoadScreenings(screeningViews);
    }
}