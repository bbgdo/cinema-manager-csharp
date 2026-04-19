namespace CinemaManager.ViewModels;

public class HallTypeFilterOption
{
    public string? Value { get; init; }
    public string Display { get; init; } = string.Empty;
}

public enum HallSortOption
{
    NameAsc,
    NameDesc,
    SeatsAsc,
    SeatsDesc
}

public class HallSortOptionItem
{
    public HallSortOption Value { get; init; }
    public string Display { get; init; } = string.Empty;
}
