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

    public static Auditorium GetAuditoriumByName(string auditoriumName)
    {
        CinemaHalls cinemaHalls = AuditoriumsDataAccess.GetAllAuditoriums();

        // Iterate over each auditorium in the list
        foreach (var auditorium in cinemaHalls.auditoriums)
        {
            // Check if the auditorium name matches the specified name
            if (auditorium.name == auditoriumName)
            {
                // Return the auditorium if found
                return auditorium;
            }
        }

        // If no matching auditorium is found, return null or throw an exception
        return null; // or throw new Exception("Auditorium not found");
    }


    private static Auditorium GetAuditoriumForMovie(string movieTitle)
    {
        var movieSchedule = MovieScheduleAccess.GetMovieSchedule();

        foreach (var movieInfo in movieSchedule)
        {
            if (movieInfo["movieTitle"] == movieTitle)
            {
                string auditoriumName = movieInfo["auditorium"];
                return GetAuditoriumByName(auditoriumName);
            }
        }

        return null;
    }

    private static bool IsSeatAvailable(Auditorium auditorium, string seatNumber)
    {
        foreach (var row in auditorium.layout)
        {
            foreach (var seat in row)
            {
                if (seat.seat == seatNumber && seat.reserved == "false")
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static void ReserveSeat(Auditorium auditorium, string seatNumber)
    {
        foreach (var row in auditorium.layout)
        {
            foreach (var seat in row)
            {
                if (seat.seat == seatNumber)
                {
                    seat.reserved = "true";
                    return;
                }
            }
        }
    }

    private static void SaveReservationToCSV(string movieTitle, DateTime date, Auditorium auditorium, string seatNumber)
    {
        // Format reservation details
        string reservationDetails = $"{movieTitle},{date:yyyy-MM-dd},{auditorium.name},{seatNumber}";

        try
        {
            // Append reservation details to the CSV file
            File.AppendAllText("C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\ReservationHistory.csv", reservationDetails + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving reservation: {ex.Message}");
        }
    }



    public static void MakeReservation()
    {
        // Prompt user to choose a date and display available movies for that date
        Console.Write("Enter date (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime selectedDate))
        {
            PrintMoviesForDay(selectedDate);

            // Prompt user to choose a movie
            Console.Write("Enter the movie title you want to reserve: ");
            string movieTitle = Console.ReadLine();

            // Get auditorium for the selected movie
            Auditorium auditorium = GetAuditoriumForMovie(movieTitle);

            if (auditorium != null)
            {
                // Display auditorium layout
                AuditoriumsPresentation.DisplayAuditoriums(new CinemaHalls { auditoriums = new Auditorium[] { auditorium } });

                // Allow user to choose a seat
                Console.Write("Enter seat number to reserve: ");
                string seatNumber = Console.ReadLine();

                // Check if the seat is available
                if (IsSeatAvailable(auditorium, seatNumber))
                {
                    // Reserve the seat
                    ReserveSeat(auditorium, seatNumber);

                    // Save reservation to CSV file
                    SaveReservationToCSV(movieTitle, selectedDate, auditorium, seatNumber);

                    Console.WriteLine("Reservation successful!");
                }
                else
                {
                    Console.WriteLine("Seat is not available.");
                }
            }
            else
            {
                Console.WriteLine("Movie not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid date format.");
        }
    }
    

}
