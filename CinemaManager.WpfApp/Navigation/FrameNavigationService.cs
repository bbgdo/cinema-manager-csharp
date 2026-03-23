using System.Windows.Controls;
using CinemaManager.Application;
using CinemaManager.ViewModels;
using CinemaManager.Wpf.Pages;

namespace CinemaManager.Wpf.Navigation;

public class FrameNavigationService(Frame frame, ICinemaService cinemaService) : INavigationService
{
    public void GoToHallDetail(int hallId)
    {
        var detail = cinemaService.GetHallDetail(hallId);
        var vm = new HallDetailViewModel(detail, this);
        frame.Navigate(new HallDetailPage(vm));
    }

    public void GoToScreeningDetail(int screeningId, string hallName)
    {
        var detail = cinemaService.GetScreeningDetail(screeningId);
        var vm = new ScreeningDetailViewModel(detail, hallName, this);
        frame.Navigate(new ScreeningDetailPage(vm));
    }

    public void GoBack() => frame.GoBack();
}
