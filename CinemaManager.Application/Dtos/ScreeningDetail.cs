namespace CinemaManager.Application.Dtos;

public class ScreeningDetail
{
    public int Id { get; init; }
    public string MovieTitle { get; init; } = string.Empty;
    public string GenreDisplay { get; init; } = string.Empty;
    public int ReleaseYear { get; init; }
    public string StartTimeDisplay { get; init; } = string.Empty;
    public string EndTimeDisplay { get; init; } = string.Empty;
    public int DurationMinutes { get; init; }
    public string PosterFileName { get; init; } = string.Empty;
}
