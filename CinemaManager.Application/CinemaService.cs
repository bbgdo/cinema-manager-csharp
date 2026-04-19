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
                HallTypeDisplay = HallTypeFormatter.Format(h.HallType),
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
            HallTypeDisplay = HallTypeFormatter.Format(hall.HallType),
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

    public async Task<HallEditModel> GetHallForEditAsync(int hallId)
    {
        var hall = await repository.GetHallByIdAsync(hallId)
            ?? throw new InvalidOperationException($"Hall {hallId} not found.");
        return new HallEditModel
        {
            Name = hall.Name,
            SeatsCount = hall.SeatsCount,
            HallType = hall.HallType
        };
    }

    public async Task<int> AddHallAsync(HallEditModel model)
    {
        var hall = new CinemaHall(0, model.Name.Trim(), model.SeatsCount, model.HallType);
        return await repository.AddHallAsync(hall);
    }

    public async Task UpdateHallAsync(int hallId, HallEditModel model)
    {
        var hall = await repository.GetHallByIdAsync(hallId)
            ?? throw new InvalidOperationException($"Hall {hallId} not found.");
        hall.Name = model.Name.Trim();
        hall.SeatsCount = model.SeatsCount;
        hall.HallType = model.HallType;
        await repository.UpdateHallAsync(hall);
    }

    public Task DeleteHallAsync(int hallId) =>
        repository.DeleteHallAsync(hallId);

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
