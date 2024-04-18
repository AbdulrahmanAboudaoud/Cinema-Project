using Newtonsoft.Json;

public static class AuditoriumsAccess
{
    public static void ShowAllAuditoriums()
    {
        string json = File.ReadAllText("C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\CinemaHalls.json");

        CinemaHalls cinemaHalls = JsonConvert.DeserializeObject<CinemaHalls>(json);

        foreach (var auditorium in cinemaHalls.auditoriums)
        {
            Console.WriteLine($"Auditorium: {auditorium.name}");
            Console.WriteLine("");

            // Print the layout upside down
            for (int i = auditorium.layout.Length - 1; i >= 0; i--)
            {
                var row = auditorium.layout[i];
                foreach (var seat in row)
                {
                    if (seat.reserved == "unavailable")
                    {
                        Console.Write("     ");
                    }
                    else
                    {
                        ConsoleColor color;
                        switch (seat.PriceRange)
                        {
                            case "low":
                                color = ConsoleColor.Blue;
                                break;
                            case "Medium":
                                color = ConsoleColor.DarkYellow;
                                break;
                            case "high":
                                color = ConsoleColor.DarkMagenta;
                                break;
                            default:
                                color = ConsoleColor.White;
                                break;
                        }
                        Console.ForegroundColor = color;
                        Console.Write("[");
                        if (seat.reserved == "false")
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }
                        else if (seat.reserved == "true")
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        }
                        Console.Write($"{seat.seat}");
                        Console.ForegroundColor = color;
                        Console.Write("] ");
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            if (auditorium.name == "Auditorium 1")
            {
                Console.WriteLine(new string(' ', 18) + "[ SCREEN ]");
            }
            else if (auditorium.name == "Auditorium 3")
            {
                Console.WriteLine(new string(' ', 58) + "[ SCREEN ]");
            }
            else
            {
                Console.WriteLine(new string(' ', 32) + "[ SCREEN ]");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
