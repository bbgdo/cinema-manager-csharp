using CinemaManager.Models;

namespace CinemaManager.ViewModels;

public class HallTypeOption
{
    public HallType Value { get; init; }
    public string Display { get; init; } = string.Empty;
}
