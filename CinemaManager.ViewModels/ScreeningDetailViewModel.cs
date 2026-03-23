using System.Windows.Input;
using CinemaManager.Application.Dtos;

namespace CinemaManager.ViewModels;

public class ScreeningDetailViewModel
{
    public ScreeningDetail Screening { get; }
    public string HallName { get; }
    public ICommand BackCommand { get; }

    public ScreeningDetailViewModel(ScreeningDetail screening, string hallName, INavigationService navigation)
    {
        Screening = screening;
        HallName = hallName;
        BackCommand = new RelayCommand(() => navigation.GoBack());
    }
}
