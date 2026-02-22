namespace CinemaManager.Models;

public class Screening {
    public int Id { get; }
    public int HallId { get; }
    public string MovieTitle { get; set; }
    public MovieGenre Genre { get; set; }
    public int ReleaseYear { get; set; }
    public DateTime StartTime { get; set; }

    public int DurationMinutes { get; set; }

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