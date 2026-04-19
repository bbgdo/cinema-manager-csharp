using CinemaManager.Models;

namespace CinemaManager.Application.Dtos;

public class HallEditModel
{
    public string Name { get; set; } = string.Empty;
    public int SeatsCount { get; set; }
    public HallType HallType { get; set; }
}
