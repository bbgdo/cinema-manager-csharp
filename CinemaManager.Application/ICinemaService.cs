using CinemaManager.Application.Dtos;

namespace CinemaManager.Application;

public interface ICinemaService
{
    Task<IReadOnlyList<CinemaHallListItem>> GetHallListAsync();
    Task<CinemaHallDetail> GetHallDetailAsync(int hallId);
    Task<ScreeningDetail> GetScreeningDetailAsync(int screeningId);
}
