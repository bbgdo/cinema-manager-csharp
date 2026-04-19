using CinemaManager.Models;

namespace CinemaManager.Services;

public interface ICinemaRepository
{
    Task<IReadOnlyList<CinemaHall>> GetAllHallsAsync();
    Task<CinemaHall?> GetHallByIdAsync(int id);
    Task<IReadOnlyList<Screening>> GetScreeningsByHallAsync(int hallId);
    Task<Screening?> GetScreeningByIdAsync(int id);

    Task<int> AddHallAsync(CinemaHall hall);
    Task UpdateHallAsync(CinemaHall hall);
    Task DeleteHallAsync(int hallId);
}
