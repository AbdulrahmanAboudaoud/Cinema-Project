using Newtonsoft.Json;

public static class AuditoriumsPresentation
{
    public static void DisplayAuditoriums(CinemaHalls cinemaHalls)
    {
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
                Console.WriteLine(new string(' ', 24) + "[ SCREEN ]");
            }
            else if (auditorium.name == "Auditorium 3")
            {
                Console.WriteLine(new string(' ', 76) + "[ SCREEN ]");
            }
            else
            {
                Console.WriteLine(new string(' ', 42) + "[ SCREEN ]");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }



    public static void DisplayAuditoriumFromFile(string fileName)
    {
        try
        {
            string json = File.ReadAllText(fileName);
            var cinemaHalls = JsonConvert.DeserializeObject<CinemaHalls>(json);

            if (cinemaHalls != null && cinemaHalls.auditoriums != null && cinemaHalls.auditoriums.Length > 0)
            {
                var auditorium = cinemaHalls.auditoriums[0]; // Assuming there is only one auditorium in the JSON file

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
                    Console.WriteLine(new string(' ', 24) + "[ SCREEN ]");
                }
                else if (auditorium.name == "Auditorium 3")
                {
                    Console.WriteLine(new string(' ', 76) + "[ SCREEN ]");
                }
                else
                {
                    Console.WriteLine(new string(' ', 42) + "[ SCREEN ]");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("No auditoriums found in the JSON file.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading JSON file: " + ex.Message);
        }
    }

}