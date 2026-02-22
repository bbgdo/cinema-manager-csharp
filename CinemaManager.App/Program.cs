using CinemaManager.Services;
using CinemaManager.ViewModels;

var repository = new CinemaRepository();
var halls = repository.GetAllHallViews();

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "Cinema Screening Manager";
bool running = true;
while (running)
{
    ShowHallList(halls);

    string? input = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(input) || input.Equals("q", StringComparison.OrdinalIgnoreCase))
    {
        running = false;
        continue;
    }

    if (!int.TryParse(input, out int selectedId))
    {
        PrintError("Enter a number from the list or 'q' to exit.");        Pause();
        continue;
    }

    var chosenHall = halls.FirstOrDefault(h => h.Id == selectedId);
    if (chosenHall is null)
    {
        PrintError($"Cinema hall with ID={selectedId} not found.");        Pause();
        continue;
    }

    repository.LoadScreeningsForHall(chosenHall);

    ShowHallDetail(chosenHall);
}

Console.Clear();
Console.WriteLine("Goodbye!");

void ShowHallList(List<CinemaHallView> hallList)
{
    Console.Clear();
    PrintHeader("CINEMA HALLS");
    Console.WriteLine();

    foreach (var hall in hallList)
        Console.WriteLine("  " + hall.GetSummaryLine());

    Console.WriteLine();
    PrintDivider();
    Console.Write("  Enter the cinema hall ID or 'q' to exit: ");
}

void ShowHallDetail(CinemaHallView hall)
{
    bool inDetail = true;

    while (inDetail)
    {
        Console.Clear();
        PrintHeader($"CINEMA HALL — {hall.Name}");
        Console.WriteLine();
        Console.Write(hall.GetDetailedInfo());
        Console.WriteLine();

        if (hall.Screenings.Count == 0) {
            Console.WriteLine("  The screening schedule is empty.");        }
        else {
            PrintSubHeader("SCREENINGS");
            Console.WriteLine();

            for (int i = 0; i < hall.Screenings.Count; i++)
                Console.WriteLine($"  {i + 1,2}. {hall.Screenings[i].GetSummaryLine()}");
        }

        Console.WriteLine();
        PrintDivider();

        Console.WriteLine(hall.Screenings.Count > 0
            ? "  Enter the screening number for details, 'b' — back to the hall list."
            : "  Enter 'b' to return to the hall list.");

        Console.Write("  > ");
        string? input = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(input) || input.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            inDetail = false;
            continue;
        }

        if (!int.TryParse(input, out int screeningIndex) ||
            screeningIndex < 1 || screeningIndex > hall.Screenings.Count)
        {
            PrintError("Invalid screening number.");
            Pause();
            continue;
        }

        var screening = hall.Screenings[screeningIndex - 1];
        ShowScreeningDetail(screening, hall.Name);
    }
}

void ShowScreeningDetail(ScreeningView screening, string hallName) {
    Console.Clear();
    PrintHeader($"SCREENING — {screening.MovieTitle}");
    Console.WriteLine($"  Hall: {hallName}");
    Console.WriteLine();
    Console.Write(screening.GetDetailedInfo());
    Console.WriteLine();
    PrintDivider();
    Console.Write("  Press Enter to return...");
    Console.ReadLine();
}

void PrintHeader(string text)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"  ══ {text} ══");
    Console.ResetColor();
}

void PrintSubHeader(string text)
{
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine($"  ── {text} ──");
    Console.ResetColor();
}

void PrintDivider() =>
    Console.WriteLine("  " + new string('─', 50));

void PrintError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("  x: " + message);
    Console.ResetColor();
}

void Pause()
{
    Console.Write("  Press Enter...");
    Console.ReadLine();
}