using System.Windows.Controls;
using CinemaManager.ViewModels;

namespace CinemaManager.Wpf.Pages;

public partial class ScreeningDetailPage : Page
{
    public ScreeningDetailPage(ScreeningDetailViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
