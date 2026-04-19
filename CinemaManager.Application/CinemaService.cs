using CinemaManager.Application.Dtos;
using CinemaManager.Models;
using CinemaManager.Services;

namespace CinemaManager.Application;

public class CinemaService(ICinemaRepository repository) : ICinemaService
{
    public async Task<IReadOnlyList<CinemaHallListItem>> GetHallListAsync() =>
        (await repository.GetAllHallsAsync())
            .Select(h => new CinemaHallListItem
            {
                Id = h.Id,
                Name = h.Name,
                HallTypeDisplay = FormatHallType(h.HallType),
                SeatsCount = h.SeatsCount
            })
            .ToList();

    public async Task<CinemaHallDetail> GetHallDetailAsync(int hallId)
    {
        var hall = await repository.GetHallByIdAsync(hallId)
            ?? throw new InvalidOperationException($"Hall {hallId} not found.");
        var screenings = await repository.GetScreeningsByHallAsync(hallId);

        var total = TimeSpan.FromMinutes(screenings.Sum(s => s.DurationMinutes));

        return new CinemaHallDetail
        {
            Id = hall.Id,
            Name = hall.Name,
            HallTypeDisplay = FormatHallType(hall.HallType),
            SeatsCount = hall.SeatsCount,
            TotalDurationDisplay = $"{(int)total.TotalHours}h {total.Minutes:D2}m total",
            Screenings = screenings.Select(MapToScreeningListItem).ToList()
        };
    }

    public async Task<ScreeningDetail> GetScreeningDetailAsync(int screeningId)
    {
        var s = await repository.GetScreeningByIdAsync(screeningId)
            ?? throw new InvalidOperationException($"Screening {screeningId} not found.");

        var endTime = s.StartTime.AddMinutes(s.DurationMinutes);

        return new ScreeningDetail
        {
            Id = s.Id,
            MovieTitle = s.MovieTitle,
            GenreDisplay = FormatGenre(s.Genre),
            ReleaseYear = s.ReleaseYear,
            StartTimeDisplay = s.StartTime.ToString("dd.MM.yyyy HH:mm"),
            EndTimeDisplay = endTime.ToString("dd.MM.yyyy HH:mm"),
            DurationMinutes = s.DurationMinutes,
            PosterFileName = s.PosterFileName
        };
    }

    private static ScreeningListItem MapToScreeningListItem(Screening s)
    {
        var endTime = s.StartTime.AddMinutes(s.DurationMinutes);
        return new ScreeningListItem
        {
            Id = s.Id,
            MovieTitle = s.MovieTitle,
            TimeRange = $"{s.StartTime:HH:mm} – {endTime:HH:mm}",
            GenreDisplay = FormatGenre(s.Genre),
            DurationMinutes = s.DurationMinutes,
            PosterFileName = s.PosterFileName
        };
    }

    private static string FormatHallType(HallType hallType) => hallType switch
    {
        HallType.Standard2D => "2D",
        HallType.ThreeD     => "3D",
        HallType.Imax       => "IMAX",
        HallType.VipLounge  => "VIP",
        _                   => hallType.ToString()
    };

    private static string FormatGenre(MovieGenre genre) => genre switch
    {
        MovieGenre.Action   => "Action",
        MovieGenre.Anime    => "Anime",
        MovieGenre.Cartoon  => "Cartoon",
        MovieGenre.Comedy   => "Comedy",
        MovieGenre.Fantasy  => "Fantasy",
        MovieGenre.Drama    => "Drama",
        MovieGenre.Horror   => "Horror",
        MovieGenre.SciFi    => "Sci-Fi",
        MovieGenre.Thriller => "Thriller",
        _                   => genre.ToString()
    };
}
