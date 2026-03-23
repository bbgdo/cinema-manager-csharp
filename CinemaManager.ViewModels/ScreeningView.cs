using System.Text;
using CinemaManager.Models;

namespace CinemaManager.ViewModels;

public class ScreeningView {
    public int Id { get; }
    public string MovieTitle { get; set; }
    public MovieGenre Genre { get; set; }
    public int ReleaseYear { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public string PosterFileName { get; set; }

    public DateTime EndTime => StartTime.AddMinutes(DurationMinutes);
    public string GenreDisplay => Genre.ToDisplayName();
    public string TimeRange => $"{StartTime:HH:mm} – {EndTime:HH:mm}";

    public ScreeningView(Screening source)
    {
        Id = source.Id;
        MovieTitle = source.MovieTitle;
        Genre = source.Genre;
        ReleaseYear = source.ReleaseYear;
        StartTime = source.StartTime;
        DurationMinutes = source.DurationMinutes;
        PosterFileName = source.PosterFileName;
    }

    public string GetSummaryLine() =>
        $"[{Id,2}] {MovieTitle,-40} {StartTime:HH:mm}–{EndTime:HH:mm}  {Genre.ToDisplayName()}";

    public string GetDetailedInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"  Movie:       {MovieTitle} ({ReleaseYear})");
        sb.AppendLine($"  Genre:       {Genre.ToDisplayName()}");
        sb.AppendLine($"  Start:       {StartTime:dd.MM.yyyy HH:mm}");
        sb.AppendLine($"  End:         {EndTime:dd.MM.yyyy HH:mm}");
        sb.AppendLine($"  Duration:    {DurationMinutes} min");
        return sb.ToString();
    }
}
