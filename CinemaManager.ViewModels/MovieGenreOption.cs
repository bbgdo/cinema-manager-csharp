using CinemaManager.Models;

namespace CinemaManager.ViewModels;

public class MovieGenreOption
{
    public MovieGenre Value { get; init; }
    public string Display { get; init; } = string.Empty;
}
