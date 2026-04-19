using System.ComponentModel;
using System.Windows.Data;
using CinemaManager.Application;
using CinemaManager.Application.Dtos;

namespace CinemaManager.ViewModels;

public class HallListViewModel : ObservableObject
{
    private readonly ICinemaService _cinemaService;
    private readonly INavigationService _navigation;

    private List<CinemaHallListItem> _allHalls = [];
    private ICollectionView? _hallsView;
    private string _searchText = string.Empty;
    private HallTypeFilterOption _hallTypeFilter;
    private HallSortOption _selectedSort = HallSortOption.NameAsc;

    public ICollectionView? HallsView
    {
        get => _hallsView;
        private set => SetProperty(ref _hallsView, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
                _hallsView?.Refresh();
        }
    }

    public HallTypeFilterOption HallTypeFilter
    {
        get => _hallTypeFilter;
        set
        {
            if (SetProperty(ref _hallTypeFilter, value))
                _hallsView?.Refresh();
        }
    }

    public HallSortOption SelectedSort
    {
        get => _selectedSort;
        set
        {
            if (SetProperty(ref _selectedSort, value))
                ApplySort();
        }
    }

    public IReadOnlyList<HallTypeFilterOption> HallTypeFilterOptions { get; }
    public IReadOnlyList<HallSortOptionItem> SortOptions { get; }
    public RelayCommand AddHallCommand { get; }

    public HallListViewModel(ICinemaService cinemaService, INavigationService navigation)
    {
        _cinemaService = cinemaService;
        _navigation = navigation;
        HallTypeFilterOptions =
        [
            new() { Value = null, Display = "All types" },
            new() { Value = "2D",   Display = "2D" },
            new() { Value = "3D",   Display = "3D" },
            new() { Value = "IMAX", Display = "IMAX" },
            new() { Value = "VIP",  Display = "VIP" },
        ];
        SortOptions =
        [
            new() { Value = HallSortOption.NameAsc,  Display = "Name A–Z" },
            new() { Value = HallSortOption.NameDesc, Display = "Name Z–A" },
            new() { Value = HallSortOption.SeatsAsc,  Display = "Seats ↑" },
            new() { Value = HallSortOption.SeatsDesc, Display = "Seats ↓" },
        ];
        _hallTypeFilter = HallTypeFilterOptions[0];
        AddHallCommand = new RelayCommand(() => _navigation.GoToHallEdit(null));
    }

    public async Task LoadAsync() => await RunAsync(async () =>
    {
        _allHalls = [.. await _cinemaService.GetHallListAsync()];
        var view = CollectionViewSource.GetDefaultView(_allHalls);
        view.Filter = FilterHall;
        HallsView = view;
        ApplySort();
    });

    public void OnHallSelected(CinemaHallListItem hall) => _navigation.GoToHallDetail(hall.Id);

    private bool FilterHall(object item)
    {
        if (item is not CinemaHallListItem h) return false;
        if (!string.IsNullOrWhiteSpace(SearchText) &&
            !h.Name.Contains(SearchText.Trim(), StringComparison.OrdinalIgnoreCase))
            return false;
        if (_hallTypeFilter.Value is { } typeVal && h.HallTypeDisplay != typeVal)
            return false;
        return true;
    }

    private void ApplySort()
    {
        if (_hallsView is null) return;
        _hallsView.SortDescriptions.Clear();
        _hallsView.SortDescriptions.Add(SelectedSort switch
        {
            HallSortOption.NameAsc   => new SortDescription(nameof(CinemaHallListItem.Name), ListSortDirection.Ascending),
            HallSortOption.NameDesc  => new SortDescription(nameof(CinemaHallListItem.Name), ListSortDirection.Descending),
            HallSortOption.SeatsAsc  => new SortDescription(nameof(CinemaHallListItem.SeatsCount), ListSortDirection.Ascending),
            HallSortOption.SeatsDesc => new SortDescription(nameof(CinemaHallListItem.SeatsCount), ListSortDirection.Descending),
            _ => new SortDescription(nameof(CinemaHallListItem.Name), ListSortDirection.Ascending)
        });
    }
}
