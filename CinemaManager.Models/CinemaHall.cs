namespace CinemaManager.Models;

public class CinemaHall {
    public int Id { get; }

    public string Name { get; set; }
    public int SeatsCount { get; set; }
    public HallType HallType { get; set; }

    public CinemaHall(int id, string name, int seatsCount, HallType hallType)
    {
        Id = id;
        Name = name;
        SeatsCount = seatsCount;
        HallType = hallType;
    }
}