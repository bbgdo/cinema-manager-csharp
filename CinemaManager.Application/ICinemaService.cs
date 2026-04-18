using CinemaManager.Application.Dtos;

namespace CinemaManager.Application;

public interface ICinemaService
{
    IReadOnlyList<CinemaHallListItem> GetHallList();
    CinemaHallDetail GetHallDetail(int hallId);
    ScreeningDetail GetScreeningDetail(int screeningId);
}
