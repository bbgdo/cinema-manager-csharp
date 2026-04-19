namespace CinemaManager.Application.Dtos;

public class ScreeningListItem
{
    public int Id { get; init; }
    public string MovieTitle { get; init; } = string.Empty;
    public string TimeRange { get; init; } = string.Empty;
    public string GenreDisplay { get; init; } = string.Empty;
    public int DurationMinutes { get; init; }
    public string PosterFileName { get; init; } = string.Empty;
    public DateTime StartTime { get; init; }
}
