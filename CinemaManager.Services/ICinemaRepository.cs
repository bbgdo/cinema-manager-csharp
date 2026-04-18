using CinemaManager.Models;

namespace CinemaManager.Services;

public interface ICinemaRepository
{
    IReadOnlyList<CinemaHall> GetAllHalls();
    IReadOnlyList<Screening> GetScreeningsByHall(int hallId);
    Screening? GetScreeningById(int id);
}
