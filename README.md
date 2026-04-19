# CinemaManager

CinemaManager is a WPF desktop application for managing cinema halls and their screenings. It is built on .NET 10, WPF, and EF Core SQLite, and represents the final state of the laboratory work after the Lab 4 storage, navigation, async, and cleanup iteration.

## Features

| Feature | Status |
|---|---|
| Browse cinema halls | Hall list on startup |
| Browse screenings of a hall | Screenings list inside hall detail |
| Screening details | Separate detail page |
| CRUD for halls | Add, edit, delete |
| CRUD for screenings | Add, edit, delete |
| Cascade delete | Deleting a hall deletes its screenings |
| Search, filter, sort for halls | Search by name, filter by hall type, sort by name/seats |
| Search, filter, sort for screenings | Search by title, filter by genre, sort by title/time/duration |
| Async storage | Task-based repository and service methods |
| Busy and error UX | Blocking busy overlay and error banner on each page |
| Navigation | Single-window Frame navigation |
| UI theme | Dark WPF theme with shared application resources |

## Projects

| Project | Type | Purpose |
|---|---|---|
| CinemaManager.Models | Class Library (net10.0) | Domain entities - CinemaHall, Screening, enums |
| CinemaManager.Services | Class Library (net10.0) | EF Core DbContext, repository, database initializer, seed data |
| CinemaManager.Application | Class Library (net10.0) | Service layer, DTOs, formatters |
| CinemaManager.ViewModels | Class Library (net10.0-windows, UseWPF) | MVVM ViewModels, ObservableObject, commands, navigation/dialog abstractions |
| CinemaManager.WpfApp | WPF App (net10.0-windows) | Pages, XAML, FrameNavigationService, DialogService, converters, DI composition root |

`CinemaManager.ViewModels` targets `net10.0-windows` and enables WPF because it uses `ICollectionView` and `CollectionViewSource` for the canonical Microsoft filter/sort pattern.

## Project References

Actual project references from the `.csproj` files:

```text
CinemaManager.WpfApp
|-- CinemaManager.ViewModels
|   `-- CinemaManager.Application
|       |-- CinemaManager.Services
|       |   `-- CinemaManager.Models
|       `-- CinemaManager.Models
|-- CinemaManager.Application
|   |-- CinemaManager.Services
|   |   `-- CinemaManager.Models
|   `-- CinemaManager.Models
`-- CinemaManager.Services
    `-- CinemaManager.Models
```

`CinemaManager.Models` is the lowest-level project. `CinemaManager.WpfApp` is the composition root and references Services directly only to configure EF Core, the repository, and database initialization in `App.xaml.cs`.

## Architecture

### Layers

The storage layer is `CinemaManager.Services`. It owns `CinemaDbContext`, `ICinemaRepository`, `CinemaRepository`, `DatabaseInitializer`, and `SeedData`.

The application layer is `CinemaManager.Application`. It exposes `ICinemaService`, `CinemaService`, DTOs, and formatter helpers. ViewModels consume this layer instead of working with EF Core or repository objects.

The presentation layer is split between `CinemaManager.ViewModels` and `CinemaManager.WpfApp`. WPF pages bind to ViewModels and DTO-shaped data. UI pages do not use domain models directly; the direct WpfApp reference to Services is limited to composition and startup initialization.

### MVVM

ViewModels inherit from `ObservableObject`, which provides `INotifyPropertyChanged`, `IsBusy`, `ErrorMessage`, `HasError`, `SetProperty`, and `RunAsync(Func<Task>)`.

Pages construct their DataContext from an injected ViewModel instance. Each page's `Loaded` handler calls `await vm.LoadAsync()` and guards with `if (_vm.IsBusy) return`.

Async UI actions use `AsyncRelayCommand`, which guards against concurrent execution and disables command execution while the task is running. Synchronous navigation commands use `RelayCommand`.

Page code-behind is limited to `InitializeComponent`, `DataContext = vm`, a `Loaded` hook, and selection hooks for ListBox navigation.

### Navigation

The application uses one `Frame` inside `MainWindow`. `INavigationService` lives in `CinemaManager.ViewModels` and contains no WPF types. `FrameNavigationService` lives in `CinemaManager.WpfApp` and creates the required pages and ViewModels with runtime parameters.

The navigation surface contains five pages:

| Page | Purpose |
|---|---|
| HallListPage | Level-1 list |
| HallDetailPage | Level-1 detail with level-2 list |
| HallEditPage | Add/edit hall |
| ScreeningDetailPage | Level-2 detail |
| ScreeningEditPage | Add/edit screening |

### Storage

Storage uses SQLite through EF Core. `AddDbContextFactory<CinemaDbContext>` creates a context factory, and each repository method creates and disposes its own context per call.

The database file is stored at:

```text
%LocalAppData%\CinemaManager\cinema.db
```

Enums are stored as strings with `HasConversion<string>()`. The Hall -> Screenings relationship uses database-level cascade delete through `OnDelete(DeleteBehavior.Cascade)`.

Startup calls `DatabaseInitializer.InitializeAsync`, which uses `EnsureCreatedAsync`. Migrations are not configured in this iteration. Seed data is inserted only when `Halls.AnyAsync()` is false.

### Async

All repository and service methods are Task-based. The UI thread is not blocked during load/save/delete operations because page loading and commands await asynchronous methods.

`RunAsync` clears previous errors, enters a busy scope, catches exceptions into `ErrorMessage`, and exits the busy scope in a disposable guard. Each page binds `IsBusy` to a semi-transparent hit-test-blocking overlay and binds `HasError`/`ErrorMessage` to an error banner.

### Filter + Sort

Filtering and sorting are implemented with `ICollectionView` in ViewModels.

| Screen | Search | Filter | Sort |
|---|---|---|---|
| HallListPage | Hall name | HallType | Name asc/desc, Seats asc/desc |
| HallDetailPage | Movie title | Genre | Title asc/desc, StartTime asc/desc, Duration asc/desc |

Filter and sort selections are kept on the ViewModel and reapplied after data reloads triggered by CRUD navigation.

### DI

Dependency injection is configured in `App.xaml.cs` with `Microsoft.Extensions.DependencyInjection`.

Registered services:

| Registration | Implementation |
|---|---|
| `DbContextFactory<CinemaDbContext>` | SQLite connection to `%LocalAppData%\CinemaManager\cinema.db` |
| `ICinemaRepository` | `CinemaRepository` |
| `ICinemaService` | `CinemaService` |
| `IDialogService` | `DialogService` |

Pages and ViewModels are created manually in `FrameNavigationService` because they need runtime navigation parameters such as hall id, screening id, and hall name.

## Running

Requirements:

| Requirement | Version |
|---|---|
| .NET SDK | .NET 10 |
| OS | Windows |

Run the application:

```bash
dotnet run --project CinemaManager.WpfApp
```

No external database installation is required. SQLite is embedded through the EF Core SQLite provider.

On first run the app creates `%LocalAppData%\CinemaManager\cinema.db` and inserts seed data: 4 halls and 12 screenings. Later runs reuse the existing database.

To reset the application to the seeded state, close the app and delete:

```text
%LocalAppData%\CinemaManager\cinema.db
```

## Navigation Flow

```text
HallListPage
|-- click hall card -> HallDetailPage
|   |-- click screening card -> ScreeningDetailPage
|   |   `-- Edit -> ScreeningEditPage
|   |-- Edit -> HallEditPage
|   `-- + Add Screening -> ScreeningEditPage
|-- + New Hall -> HallEditPage
`-- every page Back/Cancel button -> previous page through frame.GoBack()

ScreeningDetailPage
`-- Delete -> previous HallDetailPage

HallDetailPage
`-- Delete -> previous HallListPage
```

## Lab 4 Requirements Mapping

| Requirement | Where implemented |
|---|---|
| Single-window navigation | MainWindow + Frame + FrameNavigationService (5 pages) |
| MVVM | CinemaManager.ViewModels project; ObservableObject, RunAsync, RelayCommand, AsyncRelayCommand |
| 3 layers with IoC | Services (storage), Application (services/DTOs), ViewModels/WpfApp (presentation); DI in App.xaml.cs |
| Real storage, no install | SQLite via EF Core, file at `%LocalAppData%\CinemaManager\cinema.db` |
| Seed on first run only | DatabaseInitializer.InitializeAsync - seeds only if Halls.AnyAsync() == false |
| Storage integrity (cascade delete) | CinemaDbContext.OnModelCreating - OnDelete(DeleteBehavior.Cascade) on Screening -> Hall |
| Async storage access | Every ICinemaRepository/ICinemaService method returns Task<...> |
| Non-blocking UI, progress indicator | Busy overlay in App.xaml bound to ObservableObject.IsBusy via BoolToVis |
| Error handling | ErrorBanner bound to HasError + ErrorMessage, set in RunAsync catch |
| List of level-1 entities on startup | HallListPage |
| Detail of level-1 entity | HallDetailPage |
| Add/Delete level-1 | + New Hall on HallListPage, Delete button on HallDetailPage |
| Edit level-1 | Edit button on HallDetailPage -> HallEditPage |
| List of level-2 entities inside level-1 detail | Screenings list on HallDetailPage |
| Detail of level-2 entity | ScreeningDetailPage |
| Add/Delete level-2 | + Add Screening on HallDetailPage, Delete button on ScreeningDetailPage |
| Edit level-2 | Edit button on ScreeningDetailPage -> ScreeningEditPage |
| Search/filter level-1 | HallListPage filter row - search by name, filter by HallType |
| Sort level-1 | HallListPage sort combo - Name/Seats asc/desc |
| Search/filter level-2 | HallDetailPage filter row - search by title, filter by Genre |
| Sort level-2 | HallDetailPage sort combo - Title/StartTime/Duration asc/desc |

## Notes

- `CinemaManager.ViewModels` references WPF for `ICollectionView`. This is an intentional trade-off in favor of the canonical Microsoft filter/sort pattern instead of duplicating filtering logic in code-behind.
- `DatePicker` in `ScreeningEditPage` uses the default system chrome. The popup is light-themed, but it is functional; a custom dark ControlTemplate is deferred as a cosmetic improvement.
- EF Core migrations are not configured. The app uses `EnsureCreatedAsync`; if the schema changes, delete `cinema.db`. For a production scenario, add migrations.
