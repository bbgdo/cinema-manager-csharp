# CinemaManager

WPF app for browsing cinema halls and their screening schedules. Lab work 3 — 3-layer architecture + MVVM.

## Projects

| Project | Type | Purpose |
|---|---|---|
| `CinemaManager.Models` | Class Library | DB models — raw data only: `CinemaHall`, `Screening`, enums |
| `CinemaManager.Services` | Class Library | Repository layer — `ICinemaRepository`, `CinemaRepository`, `FakeDataStorage` |
| `CinemaManager.Application` | Class Library | Service layer — `ICinemaService`, `CinemaService`, DTOs |
| `CinemaManager.ViewModels` | Class Library | MVVM ViewModels, `INavigationService`, `RelayCommand` |
| `CinemaManager.WpfApp` | WPF App | 3 pages, `FrameNavigationService`, value converters |

## Project references

```
Models ← Services ← Application ← ViewModels ← WpfApp
                                   ↗
                     Application ←
```

## Architecture notes

**3 layers.** Repository layer returns raw models. Service layer maps them to DTOs and handles all formatting. WpfApp works only with DTOs — it never imports `CinemaManager.Models` or `CinemaManager.Services` types directly.

**MVVM.** ViewModels have no WPF dependencies. Code-behind files contain only `InitializeComponent`, `DataContext = vm`, and the `SelectionChanged` handler (kept there because clearing `SelectedItem` cleanly has no pure MVVM equivalent in WPF).

**Navigation.** `INavigationService` is defined in the ViewModels project so ViewModels can trigger navigation without referencing `System.Windows.Controls.Frame`. `FrameNavigationService` in WpfApp implements it.

**DI.** `ICinemaRepository` and `ICinemaService` are wired in `App.xaml.cs` via `Microsoft.Extensions.DependencyInjection`. Pages are constructed manually since they receive ViewModel instances, not resolved services.

**Seed data.** `FakeDataStorage` is `internal` — nothing outside `CinemaManager.Services` can touch it.

## Running

```bash
dotnet run --project CinemaManager.WpfApp
```

## Navigation

```
Hall list → click a hall card → Hall detail with screenings → click a screening card → Screening detail → Back
```
