using System.Windows.Controls;
using CinemaManager.Application;
using CinemaManager.ViewModels;
using CinemaManager.Wpf.Pages;

namespace CinemaManager.Wpf.Navigation;

public class FrameNavigationService(Frame frame, ICinemaService cinemaService, IDialogService dialogService) : INavigationService
{
    public void GoToHallDetail(int hallId)
    {
        var vm = new HallDetailViewModel(hallId, cinemaService, this, dialogService);
        frame.Navigate(new HallDetailPage(vm));
    }

    public void GoToScreeningDetail(int screeningId, int hallId, string hallName)
    {
        var vm = new ScreeningDetailViewModel(screeningId, hallId, hallName, cinemaService, this, dialogService);
        frame.Navigate(new ScreeningDetailPage(vm));
    }

    public void GoToScreeningEdit(int? screeningId, int hallId, string hallName)
    {
        var vm = new ScreeningEditViewModel(screeningId, hallId, hallName, cinemaService, this);
        frame.Navigate(new ScreeningEditPage(vm));
    }

    public void GoToHallEdit(int? hallId)
    {
        var vm = new HallEditViewModel(hallId, cinemaService, this);
        frame.Navigate(new HallEditPage(vm));
    }

    public void GoBack() => frame.GoBack();
}
