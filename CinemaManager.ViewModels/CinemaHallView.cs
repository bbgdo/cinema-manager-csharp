using System.Text;
using CinemaManager.Models;

namespace CinemaManager.ViewModels;

public class CinemaHallView {
    public int Id { get; }
    public string Name { get; set; }
    public int SeatsCount { get; set; }
    public HallType HallType { get; set; }
    
    private List<ScreeningView>? _screenings;

    public IReadOnlyList<ScreeningView> Screenings => _screenings ?? [];

    public bool ScreeningsLoaded => _screenings is not null;

    public TimeSpan TotalScreeningsDuration =>
        TimeSpan.FromMinutes(Screenings.Sum(s => s.DurationMinutes));

    public CinemaHallView(CinemaHall source)
    {
        Id = source.Id;
        Name = source.Name;
        SeatsCount = source.SeatsCount;
        HallType = source.HallType;
    }

    public void LoadScreenings(IEnumerable<ScreeningView> screenings)
    {
        if (ScreeningsLoaded) return;
        _screenings = [.. screenings];
    }

    public string GetSummaryLine() =>
        $"[{Id}] {Name,-20} {HallType.ToDisplayName(),-6}  {SeatsCount} місць";

    public string GetDetailedInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"  Name:            {Name}");
        sb.AppendLine($"  Hall Type:       {HallType.ToDisplayName()}");
        sb.AppendLine($"  Number of Seats: {SeatsCount}");

        if (ScreeningsLoaded)
        {
            var total = TotalScreeningsDuration;
            sb.AppendLine($"  Screenings:     {_screenings!.Count}");
            sb.AppendLine($"  Total Duration: {(int)total.TotalHours}h {total.Minutes:D2}m");
        }

        return sb.ToString();
    }
}