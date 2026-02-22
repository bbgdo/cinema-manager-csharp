# CinemaManager

Console app for browsing cinema halls and their screening schedules. Lab work 1 — C# basics and OOP.

## Projects

| Project | Type | Purpose |
|---|---|---|
| `CinemaManager.Models` | Class Library | Storage models — raw data only, no computed fields |
| `CinemaManager.ViewModels` | Class Library | Display/edit models — computed fields, formatting helpers |
| `CinemaManager.Services` | Class Library | Repository and in-memory seed data |
| `CinemaManager.App` | Console App | Entry point, all UI logic |

## Project references

```
App → Services, ViewModels
Services → Models, ViewModels
ViewModels → Models
```

## Architecture notes

**Storage vs view models.** `CinemaHall` and `Screening` hold only what would be stored in a database — no collections, no computed fields. `CinemaHallView` and `ScreeningView` wrap those and add things like `EndTime` and `TotalScreeningsDuration`.

**Seed data access.** `FakeDataStorage` is `internal`, so nothing outside `CinemaManager.Services` can touch it directly. All data goes through `CinemaRepository`.

**Lazy loading.** Screenings for a hall are loaded only when the user selects that hall. Calling `LoadScreeningsForHall` more than once on the same instance is a no-op.

## Running

```bash
dotnet run --project CinemaManager.App
```

## Navigation

```
Hall list       → enter hall ID to open detail
Hall detail     → enter screening number for details, b to go back
Screening detail→ Enter to go back
Hall list       → q to quit
```