using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using CinemaManager.Application;
using CinemaManager.Application.Dtos;
using CinemaManager.Models;

namespace CinemaManager.ViewModels;

public class HallDetailViewModel : ObservableObject
{
    private readonly int _hallId;
    private readonly ICinemaService _cinemaService;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogService;
    private readonly RelayCommand _addScreeningCommand;

    private CinemaHallDetail? _hall;
    private List<ScreeningListItem> _allScreenings = [];
    private ICollectionView? _screeningsView;
    private string _screeningSearchText = string.Empty;
    private MovieGenreFilterOption _genreFilter;
    private ScreeningSortOption _selectedScreeningSort = ScreeningSortOption.TimeAsc;

    public CinemaHallDetail? Hall
    {
        get => _hall;
        private set
        {
            if (SetProperty(ref _hall, value))
                _addScreeningCommand.RaiseCanExecuteChanged();
        }
    }

    public ICollectionView? ScreeningsView
    {
        get => _screeningsView;
        private set => SetProperty(ref _screeningsView, value);
    }

    public string ScreeningSearchText
    {
        get => _screeningSearchText;
        set
        {
            if (SetProperty(ref _screeningSearchText, value))
                _screeningsView?.Refresh();
        }
    }

    public MovieGenreFilterOption GenreFilter
    {
        get => _genreFilter;
        set
        {
            if (SetProperty(ref _genreFilter, value))
                _screeningsView?.Refresh();
        }
    }

    public ScreeningSortOption SelectedScreeningSort
    {
        get => _selectedScreeningSort;
        set
        {
            if (SetProperty(ref _selectedScreeningSort, value))
                ApplyScreeningSort();
        }
    }

    public IReadOnlyList<MovieGenreFilterOption> GenreFilterOptions { get; }
    public IReadOnlyList<ScreeningSortOptionItem> ScreeningSortOptions { get; }

    public ICommand BackCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand AddScreeningCommand => _addScreeningCommand;

    public HallDetailViewModel(int hallId, ICinemaService cinemaService, INavigationService navigation, IDialogService dialogService)
    {
        _hallId = hallId;
        _cinemaService = cinemaService;
        _navigation = navigation;
        _dialogService = dialogService;

        GenreFilterOptions = new MovieGenreFilterOption[] { new() { Value = null, Display = "All genres" } }
            .Concat(Enum.GetValues<MovieGenre>()
                .Select(g => new MovieGenreFilterOption { Value = MovieGenreFormatter.Format(g), Display = MovieGenreFormatter.Format(g) }))
            .ToList();
        ScreeningSortOptions =
        [
            new() { Value = ScreeningSortOption.TimeAsc,      Display = "Time ↑" },
            new() { Value = ScreeningSortOption.TimeDesc,     Display = "Time ↓" },
            new() { Value = ScreeningSortOption.TitleAsc,     Display = "Title A–Z" },
            new() { Value = ScreeningSortOption.TitleDesc,    Display = "Title Z–A" },
            new() { Value = ScreeningSortOption.DurationAsc,  Display = "Duration ↑" },
            new() { Value = ScreeningSortOption.DurationDesc, Display = "Duration ↓" },
        ];
        _genreFilter = GenreFilterOptions[0];

        BackCommand = new RelayCommand(() => _navigation.GoBack());
        EditCommand = new RelayCommand(() => _navigation.GoToHallEdit(_hallId));
        DeleteCommand = new AsyncRelayCommand(DeleteAsync);
        _addScreeningCommand = new RelayCommand(
            () => _navigation.GoToScreeningEdit(null, _hallId, Hall?.Name ?? string.Empty),
            () => Hall is not null);
    }

    public async Task LoadAsync() => await RunAsync(async () =>
    {
        Hall = await _cinemaService.GetHallDetailAsync(_hallId);
        _allScreenings = [.. Hall.Screenings];
        var view = CollectionViewSource.GetDefaultView(_allScreenings);
        view.Filter = FilterScreening;
        ScreeningsView = view;
        ApplyScreeningSort();
    });

    public void OnScreeningSelected(ScreeningListItem screening) =>
        _navigation.GoToScreeningDetail(screening.Id, _hallId, Hall?.Name ?? string.Empty);

    private bool FilterScreening(object item)
    {
        if (item is not ScreeningListItem s) return false;
        if (!string.IsNullOrWhiteSpace(ScreeningSearchText) &&
            !s.MovieTitle.Contains(ScreeningSearchText.Trim(), StringComparison.OrdinalIgnoreCase))
            return false;
        if (_genreFilter.Value is { } genreVal && s.GenreDisplay != genreVal)
            return false;
        return true;
    }

    private void ApplyScreeningSort()
    {
        if (_screeningsView is null) return;
        _screeningsView.SortDescriptions.Clear();
        _screeningsView.SortDescriptions.Add(SelectedScreeningSort switch
        {
            ScreeningSortOption.TitleAsc     => new SortDescription(nameof(ScreeningListItem.MovieTitle), ListSortDirection.Ascending),
            ScreeningSortOption.TitleDesc    => new SortDescription(nameof(ScreeningListItem.MovieTitle), ListSortDirection.Descending),
            ScreeningSortOption.TimeAsc      => new SortDescription(nameof(ScreeningListItem.StartTime), ListSortDirection.Ascending),
            ScreeningSortOption.TimeDesc     => new SortDescription(nameof(ScreeningListItem.StartTime), ListSortDirection.Descending),
            ScreeningSortOption.DurationAsc  => new SortDescription(nameof(ScreeningListItem.DurationMinutes), ListSortDirection.Ascending),
            ScreeningSortOption.DurationDesc => new SortDescription(nameof(ScreeningListItem.DurationMinutes), ListSortDirection.Descending),
            _ => new SortDescription(nameof(ScreeningListItem.StartTime), ListSortDirection.Ascending)
        });
    }

    private async Task DeleteAsync()
    {
        var confirmed = await _dialogService.ConfirmAsync(
            "Delete Hall",
            $"Delete '{Hall?.Name ?? "this hall"}'? All screenings in this hall will be removed.");
        if (!confirmed) return;
        await RunAsync(async () =>
        {
            await _cinemaService.DeleteHallAsync(_hallId);
            _navigation.GoBack();
        });
    }
}
