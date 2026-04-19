namespace CinemaManager.ViewModels;

public interface INavigationService
{
    void GoToHallDetail(int hallId);
    void GoToScreeningDetail(int screeningId, string hallName);
    void GoToHallEdit(int? hallId);
    void GoBack();
}
