using System.Text;
using CinemaManager.Models;

namespace CinemaManager.ViewModels;

public class ScreeningView {
    public int Id { get; }
    public int HallId { get; }
    public string MovieTitle { get; set; }
    public MovieGenre Genre { get; set; }
    public int ReleaseYear { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }

    public DateTime EndTime => StartTime.AddMinutes(DurationMinutes);

    public ScreeningView(Screening source)
    {
        Id = source.Id;
        HallId = source.HallId;
        MovieTitle = source.MovieTitle;
        Genre = source.Genre;
        ReleaseYear = source.ReleaseYear;
        StartTime = source.StartTime;
        DurationMinutes = source.DurationMinutes;
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