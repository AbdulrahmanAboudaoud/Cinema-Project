using System.Globalization;

public static class ReservationLogic
{
    public static void PrintMoviesForDay(DateTime date)
    {
        Console.WriteLine($"\nMovies for {date:yyyy-MM-dd}:");
        var movieSchedule = MovieScheduleAccess.GetMovieSchedule();

        foreach (var movieInfo in movieSchedule)
        {
            if (DateTime.TryParseExact(movieInfo["displayTime"], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime displayTime))
            {
                if (displayTime.Date == date.Date)
                {
                    Console.WriteLine($"- {movieInfo["movieTitle"]} at {displayTime:HH:mm} in {movieInfo["auditorium"]}");
                }
            }
            else
            {
                Console.WriteLine($"Error parsing display time for movie: {movieInfo["movieTitle"]}");
            }
        }
    }

    public static void MakeReservation()
    {
        Console.Write("Enter date (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime selectedDate))
        {
            PrintMoviesForDay(selectedDate);


        }
        else
        {
            Console.WriteLine("Invalid date format.");
        }
    }
}
