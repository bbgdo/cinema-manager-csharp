namespace CinemaManager.Models;

public class Screening {
    public int Id { get; set; }
    public int HallId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public MovieGenre Genre { get; set; }
    public int ReleaseYear { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public string PosterFileName { get; set; } = string.Empty;

    private Screening() { }

    public Screening(
        int id,
        int hallId,
        string movieTitle,
        MovieGenre genre,
        int releaseYear,
        DateTime startTime,
        int durationMinutes)
    {
        Id = id;
        HallId = hallId;
        MovieTitle = movieTitle;
        Genre = genre;
        ReleaseYear = releaseYear;
        StartTime = startTime;
        DurationMinutes = durationMinutes;
    }
}