using CinemaManager.Models;

namespace CinemaManager.Application.Dtos;

public class ScreeningEditModel
{
    public int HallId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public MovieGenre Genre { get; set; }
    public int ReleaseYear { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public string PosterFileName { get; set; } = string.Empty;
}
