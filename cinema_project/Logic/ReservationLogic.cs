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

            Console.Write("Enter the name of the movie to reserve: ");
            string movieToReserve = Console.ReadLine();

            var movieSchedule = MovieScheduleAccess.GetMovieSchedule();
            var selectedMovie = movieSchedule.FirstOrDefault(movie => movie["movieTitle"].Equals(movieToReserve, StringComparison.OrdinalIgnoreCase) && DateTime.TryParseExact(movie["displayTime"], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime displayTime) && displayTime.Date == selectedDate.Date);

            if (selectedMovie != null)
            {
                string auditoriumName = selectedMovie["auditorium"];
                var cinemaHalls = AuditoriumsDataAccess.GetAllAuditoriums(); // Implement a method to retrieve cinema halls
                var auditorium = cinemaHalls.auditoriums.FirstOrDefault(a => a.name.Equals(auditoriumName, StringComparison.OrdinalIgnoreCase));
                if (auditorium != null)
                {
                    Console.WriteLine($"\nAuditorium for {movieToReserve}:");
                    AuditoriumsPresentation.DisplayAuditoriums(new CinemaHalls { auditoriums = new Auditorium[] { auditorium } });
                }
                else
                {
                    Console.WriteLine("Auditorium not found for the selected movie.");
                }
            }
            else
            {
                Console.WriteLine("Movie not found for the selected date.");
            }
        }
        else
        {
            Console.WriteLine("Invalid date format.");
        }
    }

}
