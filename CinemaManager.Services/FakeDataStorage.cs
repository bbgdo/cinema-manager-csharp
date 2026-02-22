using CinemaManager.Models;

namespace CinemaManager.Services;

internal static class FakeDataStorage {
    internal static IReadOnlyList<CinemaHall> CinemaHalls { get; } = BuildHalls();
    internal static IReadOnlyList<Screening> Screenings { get; } = BuildScreenings();

    private static List<CinemaHall> BuildHalls() =>
    [
        new CinemaHall(1, "Horizon Hall", 180, HallType.Imax),
        new CinemaHall(2, "Meridian Hall", 90, HallType.Standard2D),
        new CinemaHall(3, "VIP Premium Lounge", 28, HallType.VipLounge),
        new CinemaHall(4, "This dimension Hall", 90, HallType.ThreeD),
    ];

    private static List<Screening> BuildScreenings()
    {
        var baseDate = new DateTime(2025, 6, 14);

        return
        [
            new Screening(1, 1, "Dune: Part Two", MovieGenre.SciFi, 2024, baseDate.AddHours(9, 0), 166),
            new Screening(2, 1, "Oppenheimer", MovieGenre.Drama, 2023, baseDate.AddHours(12, 30), 180),
            new Screening(3, 2, "Whiplash", MovieGenre.Drama, 2014, baseDate.AddHours(16, 0), 107),
            new Screening(4, 3, "Joker", MovieGenre.Thriller, 2019, baseDate.AddHours(18, 0), 122),
            new Screening(5, 4, "Interstellar", MovieGenre.SciFi, 2014, baseDate.AddHours(20, 30), 169),
            new Screening(6, 1, "Barbie", MovieGenre.Comedy, 2023, baseDate.AddDays(1).AddHours(9, 30), 114),
            new Screening(7, 1, "Forrest Gump", MovieGenre.Drama, 1994, baseDate.AddDays(1).AddHours(12, 0), 142),
            new Screening(8, 2, "Inception", MovieGenre.Thriller, 2010, baseDate.AddDays(1).AddHours(14, 30), 148),
            new Screening(9, 3, "Gravity", MovieGenre.SciFi, 2013, baseDate.AddDays(1).AddHours(17, 0), 91),
            new Screening(10, 4, "The Green Mile", MovieGenre.Drama, 1999, baseDate.AddDays(1).AddHours(19, 15), 189),
            new Screening(11, 2, "Spider-Man: No Way Home", MovieGenre.Action, 2021, baseDate.AddHours(11, 0), 148),
            new Screening(12, 1, "Puss in Boots: The Last Wish", MovieGenre.Cartoon, 2022, baseDate.AddHours(14, 0), 102),
        ];
    }
}

file static class DateTimeExtensions
{
    internal static DateTime AddHours(this DateTime dt, int hours, int minutes) =>
        dt.AddHours(hours).AddMinutes(minutes);
}