using CinemaManager.Models;

namespace CinemaManager.Services;

internal static class SeedData
{
    internal static IEnumerable<CinemaHall> GetHalls() =>
    [
        new(1, "Horizon Hall", 180, HallType.Imax),
        new(2, "Meridian Hall", 90, HallType.Standard2D),
        new(3, "VIP Premium Lounge", 28, HallType.VipLounge),
        new(4, "This dimension Hall", 90, HallType.ThreeD),
    ];

    internal static IEnumerable<Screening> GetScreenings()
    {
        var baseDate = new DateTime(2025, 6, 14);

        return
        [
            new Screening(1, 1, "Dune: Part Two", MovieGenre.SciFi, 2024, baseDate.AddHours(9, 0), 66) { PosterFileName = "dune_part_two.jpg" },
            new Screening(2, 1, "Oppenheimer", MovieGenre.Drama, 2023, baseDate.AddHours(12, 30), 180) { PosterFileName = "oppenheimer.jpg" },
            new Screening(3, 2, "Whiplash", MovieGenre.Drama, 2014, baseDate.AddHours(16, 0), 107) { PosterFileName = "whiplash.jpg" },
            new Screening(4, 3, "Joker", MovieGenre.Thriller, 2019, baseDate.AddHours(18, 0), 122) { PosterFileName = "joker.jpg" },
            new Screening(5, 4, "Interstellar", MovieGenre.SciFi, 2014, baseDate.AddHours(20, 30), 169) { PosterFileName = "interstellar.jpg" },
            new Screening(6, 1, "Barbie", MovieGenre.Comedy, 2023, baseDate.AddDays(1).AddHours(9, 30), 114) { PosterFileName = "barbie.jpg" },
            new Screening(7, 1, "Forrest Gump", MovieGenre.Drama, 1994, baseDate.AddDays(1).AddHours(12, 0), 142) { PosterFileName = "forrest_gump.jpg" },
            new Screening(8, 2, "Inception", MovieGenre.Thriller, 2010, baseDate.AddDays(1).AddHours(14, 30), 148) { PosterFileName = "inception.jpg" },
            new Screening(9, 3, "Gravity", MovieGenre.SciFi, 2013, baseDate.AddDays(1).AddHours(17, 0), 91) { PosterFileName = "gravity.jpg" },
            new Screening(10, 4, "The Green Mile", MovieGenre.Drama, 1999, baseDate.AddDays(1).AddHours(19, 15), 189) { PosterFileName = "the_green_mile.jpg" },
            new Screening(11, 2, "Spider-Man: No Way Home", MovieGenre.Action, 2021, baseDate.AddHours(11, 0), 148) { PosterFileName = "spider_man_no_way_home.jpg" },
            new Screening(12, 1, "Puss in Boots: The Last Wish", MovieGenre.Cartoon, 2022, baseDate.AddHours(14, 0), 102) { PosterFileName = "puss_in_boots.jpg" },
        ];
    }
}

file static class DateTimeExtensions
{
    internal static DateTime AddHours(this DateTime dt, int hours, int minutes) =>
        dt.AddHours(hours).AddMinutes(minutes);
}
