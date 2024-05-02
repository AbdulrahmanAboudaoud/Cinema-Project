using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

public static class ReservationLogic
{
    private static string jsonFolderPath = @"C:\CodingProjects\C#\Cinema\Cinema-Project\cinema_project\DataSources";

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
        return null; 
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
//-----------> Moet gefixt worden
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
//-----------> Moet gefixt worden 
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




    private static void SaveReservationToCSV(string username, string movieTitle, DateTime date, string auditoriumName, string seatNumber)
    {
        // Get movie schedule
        var movieSchedule = MovieScheduleAccess.GetMovieSchedule();

        // Find the movie schedule entry for the given movie title
        var movieInfo = movieSchedule.FirstOrDefault(m => m["movieTitle"].ToString() == movieTitle);

        if (movieInfo != null)
        {
            // Get the display time from the movie schedule
            if (DateTime.TryParseExact(movieInfo["displayTime"].ToString(), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime movieDisplayTime))
            {
                // Format reservation details
                string reservationDetails = $"{username},{movieTitle},{movieDisplayTime:yyyy-MM-dd HH:mm},{auditoriumName},{seatNumber}";

                try
                {
                    // Append reservation details to the CSV file
                    File.AppendAllText("C:\\CodingProjects\\C#\\Cinema\\Cinema-Project\\cinema_project\\DataSources\\ReservationHistory.csv", reservationDetails + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving reservation: {ex.Message}");
                }

                // Update the reserved status of the seat in the JSON file
                UpdateSeatStatusInJson(movieTitle, date, auditoriumName, seatNumber);
            }
            else
            {
                Console.WriteLine($"Error parsing display time for movie: {movieTitle}");
            }
        }
        else
        {
            Console.WriteLine($"Movie not found in schedule: {movieTitle}");
        }
    }




    public static void UpdateSeatStatusInJson(string movieTitle, DateTime displayDate, string auditorium, string seatNumber)
    {
        // Ensure displayDate includes the correct time information
        displayDate = displayDate.Date.Add(displayDate.TimeOfDay);

        // Get movie schedule
        var movieSchedule = MovieScheduleAccess.GetMovieSchedule();

        // Find the movie schedule entry for the given movie title
        var movieInfo = movieSchedule.FirstOrDefault(m => m["movieTitle"].ToString() == movieTitle);

        if (movieInfo != null)
        {
            // Get the display time from the movie schedule
            if (DateTime.TryParseExact(movieInfo["displayTime"].ToString(), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime movieDisplayTime))
            {
                // Use the display time to construct the file name
                string fileName = $"{movieTitle}-{movieDisplayTime:yyyyMMdd-HHmm}-{auditorium}.json";
                string filePath = Path.Combine(@"C:\CodingProjects\C#\Cinema\Cinema-Project\cinema_project\DataSources", fileName);

                try
                {
                    if (File.Exists(filePath))
                    {
                        // Read JSON content from file
                        string jsonContent = File.ReadAllText(filePath);

                        // Parse JSON content
                        JObject jsonObject = JObject.Parse(jsonContent);

                        // Find the auditorium in the JSON
                        JToken auditoriumToken = jsonObject["auditoriums"].FirstOrDefault(a => a["name"].ToString() == auditorium);
                        if (auditoriumToken != null)
                        {
                            // Find the seat in the auditorium
                            JToken seatToken = auditoriumToken["layout"].SelectMany(row => row)
                                .FirstOrDefault(seat => seat["seat"].ToString() == seatNumber);

                            if (seatToken != null)
                            {
                                // Update the reserved status to true
                                seatToken["reserved"] = "true";

                                // Write the updated JSON content back to the file
                                File.WriteAllText(filePath, jsonObject.ToString());

                                Console.WriteLine($"Seat {seatNumber} status updated in {fileName}.");
                            }
                            else
                            {
                                Console.WriteLine($"Seat {seatNumber} not found in {auditorium}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Auditorium {auditorium} not found in {fileName}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"File not found: {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating seat status: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Error parsing display time for movie: {movieTitle}");
            }
        }
        else
        {
            Console.WriteLine($"Movie not found in schedule: {movieTitle}");
        }
    }




     public static void MakeReservation(string username)
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
                //AuditoriumsPresentation.DisplayAuditoriums(new CinemaHalls { auditoriums = new Auditorium[] { auditorium } });
                AuditoriumsPresentation.DisplayAuditorium(auditorium);

                // Allow user to choose a seat
                Console.Write("Enter seat number to reserve: ");
                 string seatNumber = Console.ReadLine();

                 // Check if the seat is available
                 if (IsSeatAvailable(auditorium, seatNumber))
                 {
                     // Reserve the seat
                     ReserveSeat(auditorium, seatNumber);

                     // Save reservation to CSV file
                     SaveReservationToCSV(username, movieTitle, selectedDate, auditorium.name, seatNumber);

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
