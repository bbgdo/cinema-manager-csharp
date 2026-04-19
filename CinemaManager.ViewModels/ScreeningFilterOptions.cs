namespace CinemaManager.ViewModels;

public class MovieGenreFilterOption
{
    public string? Value { get; init; }
    public string Display { get; init; } = string.Empty;
}

public enum ScreeningSortOption
{
    TitleAsc,
    TitleDesc,
    TimeAsc,
    TimeDesc,
    DurationAsc,
    DurationDesc
}

public class ScreeningSortOptionItem
{
    public ScreeningSortOption Value { get; init; }
    public string Display { get; init; } = string.Empty;
}
