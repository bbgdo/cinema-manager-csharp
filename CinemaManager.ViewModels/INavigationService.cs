namespace CinemaManager.ViewModels;

public interface INavigationService
{
    void GoToHallDetail(int hallId);
    void GoToScreeningDetail(int screeningId, int hallId, string hallName);
    void GoToScreeningEdit(int? screeningId, int hallId, string hallName);
    void GoToHallEdit(int? hallId);
    void GoBack();
}
