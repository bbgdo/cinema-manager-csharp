using System.Windows.Input;
using CinemaManager.Application;
using CinemaManager.Application.Dtos;
using CinemaManager.Models;

namespace CinemaManager.ViewModels;

public class HallEditViewModel : ObservableObject
{
    private readonly int? _hallId;
    private readonly ICinemaService _cinemaService;
    private readonly INavigationService _navigation;
    private readonly AsyncRelayCommand _saveCommand;

    private string _name = string.Empty;
    private int _seatsCount = 50;
    private HallType _hallType;

    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
                _saveCommand.RaiseCanExecuteChanged();
        }
    }

    public int SeatsCount
    {
        get => _seatsCount;
        set
        {
            if (SetProperty(ref _seatsCount, value))
                _saveCommand.RaiseCanExecuteChanged();
        }
    }

    public HallType HallType
    {
        get => _hallType;
        set => SetProperty(ref _hallType, value);
    }

    public string Title => _hallId is null ? "New Hall" : "Edit Hall";
    public IReadOnlyList<HallTypeOption> HallTypeOptions { get; }
    public ICommand SaveCommand => _saveCommand;
    public ICommand CancelCommand { get; }

    public HallEditViewModel(int? hallId, ICinemaService cinemaService, INavigationService navigation)
    {
        _hallId = hallId;
        _cinemaService = cinemaService;
        _navigation = navigation;
        _hallType = Enum.GetValues<HallType>().First();
        HallTypeOptions = Enum.GetValues<HallType>()
            .Select(t => new HallTypeOption { Value = t, Display = HallTypeFormatter.Format(t) })
            .ToList();
        _saveCommand = new AsyncRelayCommand(SaveAsync, () => !string.IsNullOrWhiteSpace(Name) && SeatsCount > 0);
        CancelCommand = new RelayCommand(() => _navigation.GoBack());
    }

    public async Task LoadAsync()
    {
        if (_hallId is null) return;
        await RunAsync(async () =>
        {
            var model = await _cinemaService.GetHallForEditAsync(_hallId.Value);
            Name = model.Name;
            SeatsCount = model.SeatsCount;
            HallType = model.HallType;
        });
    }

    private async Task SaveAsync()
    {
        await RunAsync(async () =>
        {
            var model = new HallEditModel
            {
                Name = Name.Trim(),
                SeatsCount = SeatsCount,
                HallType = HallType
            };
            if (_hallId is null)
                await _cinemaService.AddHallAsync(model);
            else
                await _cinemaService.UpdateHallAsync(_hallId.Value, model);
            _navigation.GoBack();
        });
    }
}
