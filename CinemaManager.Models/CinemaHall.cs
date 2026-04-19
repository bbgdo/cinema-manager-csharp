namespace CinemaManager.Models;

public class CinemaHall {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SeatsCount { get; set; }
    public HallType HallType { get; set; }

    private CinemaHall() { }

    public CinemaHall(int id, string name, int seatsCount, HallType hallType)
    {
        Id = id;
        Name = name;
        SeatsCount = seatsCount;
        HallType = hallType;
    }
}