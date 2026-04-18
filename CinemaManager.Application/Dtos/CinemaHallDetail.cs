namespace CinemaManager.Application.Dtos;

public class CinemaHallDetail
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string HallTypeDisplay { get; init; } = string.Empty;
    public int SeatsCount { get; init; }
    public string TotalDurationDisplay { get; init; } = string.Empty;
    public IReadOnlyList<ScreeningListItem> Screenings { get; init; } = [];
}
