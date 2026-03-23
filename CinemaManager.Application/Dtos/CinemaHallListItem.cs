namespace CinemaManager.Application.Dtos;

public class CinemaHallListItem
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string HallTypeDisplay { get; init; } = string.Empty;
    public int SeatsCount { get; init; }
}
