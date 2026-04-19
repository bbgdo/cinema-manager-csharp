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
            GenreDisplay = MovieGenreFormatter.Format(s.Genre),
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

    public async Task<ScreeningEditModel> GetScreeningForEditAsync(int screeningId)
    {
        var s = await repository.GetScreeningByIdAsync(screeningId)
            ?? throw new InvalidOperationException($"Screening {screeningId} not found.");
        return new ScreeningEditModel
        {
            HallId = s.HallId,
            MovieTitle = s.MovieTitle,
            Genre = s.Genre,
            ReleaseYear = s.ReleaseYear,
            StartTime = s.StartTime,
            DurationMinutes = s.DurationMinutes,
            PosterFileName = s.PosterFileName
        };
    }

    public async Task<int> AddScreeningAsync(ScreeningEditModel model)
    {
        var screening = new Screening(0, model.HallId, model.MovieTitle.Trim(), model.Genre, model.ReleaseYear, model.StartTime, model.DurationMinutes);
        screening.PosterFileName = model.PosterFileName;
        return await repository.AddScreeningAsync(screening);
    }

    public async Task UpdateScreeningAsync(int screeningId, ScreeningEditModel model)
    {
        var screening = await repository.GetScreeningByIdAsync(screeningId)
            ?? throw new InvalidOperationException($"Screening {screeningId} not found.");
        screening.MovieTitle = model.MovieTitle.Trim();
        screening.Genre = model.Genre;
        screening.ReleaseYear = model.ReleaseYear;
        screening.StartTime = model.StartTime;
        screening.DurationMinutes = model.DurationMinutes;
        screening.PosterFileName = model.PosterFileName;
        await repository.UpdateScreeningAsync(screening);
    }

    public Task DeleteScreeningAsync(int screeningId) =>
        repository.DeleteScreeningAsync(screeningId);

    private static ScreeningListItem MapToScreeningListItem(Screening s)
    {
        var endTime = s.StartTime.AddMinutes(s.DurationMinutes);
        return new ScreeningListItem
        {
            Id = s.Id,
            MovieTitle = s.MovieTitle,
            TimeRange = $"{s.StartTime:HH:mm} – {endTime:HH:mm}",
            GenreDisplay = MovieGenreFormatter.Format(s.Genre),
            DurationMinutes = s.DurationMinutes,
            PosterFileName = s.PosterFileName,
            StartTime = s.StartTime
        };
    }

}
