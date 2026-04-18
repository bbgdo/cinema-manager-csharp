using CinemaManager.Models;

namespace CinemaManager.Services;

public class CinemaRepository : ICinemaRepository {
    public IReadOnlyList<CinemaHall> GetAllHalls() => FakeDataStorage.CinemaHalls;

    public IReadOnlyList<Screening> GetScreeningsByHall(int hallId) =>
        FakeDataStorage.Screenings.Where(s => s.HallId == hallId).ToList();

    public Screening? GetScreeningById(int id) =>
        FakeDataStorage.Screenings.FirstOrDefault(s => s.Id == id);
}
