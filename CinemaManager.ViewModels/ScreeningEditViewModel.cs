using System.Globalization;
using System.Windows.Input;
using CinemaManager.Application;
using CinemaManager.Application.Dtos;
using CinemaManager.Models;

namespace CinemaManager.ViewModels;

public class ScreeningEditViewModel : ObservableObject
{
    private readonly int? _screeningId;
    private readonly int _hallId;
    private readonly string _hallName;
    private readonly ICinemaService _cinemaService;
    private readonly INavigationService _navigation;
    private readonly AsyncRelayCommand _saveCommand;

    private string _movieTitle = string.Empty;
    private MovieGenre _genre;
    private int _releaseYear = DateTime.Today.Year;
    private DateTime _startDate = DateTime.Today;
    private string _startTimeText = "18:00";
    private int _durationMinutes = 90;
    private string _posterFileName = string.Empty;

    public string MovieTitle
    {
        get => _movieTitle;
        set { if (SetProperty(ref _movieTitle, value)) _saveCommand.RaiseCanExecuteChanged(); }
    }

    public MovieGenre Genre
    {
        get => _genre;
        set => SetProperty(ref _genre, value);
    }

    public int ReleaseYear
    {
        get => _releaseYear;
        set { if (SetProperty(ref _releaseYear, value)) _saveCommand.RaiseCanExecuteChanged(); }
    }

    public DateTime StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value);
    }

    public string StartTimeText
    {
        get => _startTimeText;
        set { if (SetProperty(ref _startTimeText, value)) _saveCommand.RaiseCanExecuteChanged(); }
    }

    public int DurationMinutes
    {
        get => _durationMinutes;
        set { if (SetProperty(ref _durationMinutes, value)) _saveCommand.RaiseCanExecuteChanged(); }
    }

    public string PosterFileName
    {
        get => _posterFileName;
        set => SetProperty(ref _posterFileName, value);
    }

    public string Title => _screeningId is null
        ? $"New screening in {_hallName}"
        : $"Edit screening in {_hallName}";

    public IReadOnlyList<MovieGenreOption> GenreOptions { get; }
    public ICommand SaveCommand => _saveCommand;
    public ICommand CancelCommand { get; }

    public ScreeningEditViewModel(int? screeningId, int hallId, string hallName, ICinemaService cinemaService, INavigationService navigation)
    {
        _screeningId = screeningId;
        _hallId = hallId;
        _hallName = hallName;
        _cinemaService = cinemaService;
        _navigation = navigation;
        _genre = Enum.GetValues<MovieGenre>().First();
        GenreOptions = Enum.GetValues<MovieGenre>()
            .Select(g => new MovieGenreOption { Value = g, Display = MovieGenreFormatter.Format(g) })
            .ToList();
        _saveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        CancelCommand = new RelayCommand(() => _navigation.GoBack());
    }

    public async Task LoadAsync()
    {
        if (_screeningId is null) return;
        await RunAsync(async () =>
        {
            var model = await _cinemaService.GetScreeningForEditAsync(_screeningId.Value);
            MovieTitle = model.MovieTitle;
            Genre = model.Genre;
            ReleaseYear = model.ReleaseYear;
            StartDate = model.StartTime.Date;
            StartTimeText = model.StartTime.ToString("HH:mm");
            DurationMinutes = model.DurationMinutes;
            PosterFileName = model.PosterFileName;
        });
    }

    private bool CanSave() =>
        !string.IsNullOrWhiteSpace(MovieTitle) &&
        DurationMinutes > 0 &&
        ReleaseYear > 1880 &&
        TryParseTime(StartTimeText, out _);

    private async Task SaveAsync()
    {
        TryParseTime(StartTimeText, out var time);
        var startTime = StartDate.Date + time;
        await RunAsync(async () =>
        {
            var model = new ScreeningEditModel
            {
                HallId = _hallId,
                MovieTitle = MovieTitle.Trim(),
                Genre = Genre,
                ReleaseYear = ReleaseYear,
                StartTime = startTime,
                DurationMinutes = DurationMinutes,
                PosterFileName = PosterFileName
            };
            if (_screeningId is null)
                await _cinemaService.AddScreeningAsync(model);
            else
                await _cinemaService.UpdateScreeningAsync(_screeningId.Value, model);
            _navigation.GoBack();
        });
    }

    private static bool TryParseTime(string? value, out TimeSpan result)
    {
        result = default;
        if (!TimeSpan.TryParseExact(value ?? string.Empty, "h\\:mm", CultureInfo.InvariantCulture, out result))
            return false;
        return result >= TimeSpan.Zero && result < TimeSpan.FromHours(24);
    }
}
