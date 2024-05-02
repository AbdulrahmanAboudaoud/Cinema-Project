using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;


public static class ReservationLogic
{
    private static string jsonFolderPath = @"C:\Users\abdul\OneDrive\Documents\GitHub\Cinema-Project\cinema_project\DataSources\";

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

    private static JArray GetMovieSchedule()
    {
        try
        {
            string jsonContent = File.ReadAllText(Path.Combine(jsonFolderPath, "movieSchedule.json"));
            return JsonConvert.DeserializeObject<JArray>(jsonContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading movie schedule: {ex.Message}");
            return null;
        }
    }

    private static bool IsSeatAvailable(JObject auditoriumData, string seatNumber)
    {
        var auditorium = auditoriumData["auditoriums"][0];
        foreach (var row in auditorium["layout"])
        {
            foreach (var seat in row)
            {
                if (seat["seat"].ToString() == seatNumber && seat["reserved"].ToString() == "false")
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static void ReserveSeatAndUpdateFile(JObject auditoriumData, string seatNumber, string fileName)
    {
        var auditorium = auditoriumData["auditoriums"][0];
        foreach (var row in auditorium["layout"])
        {
            foreach (var seat in row)
            {
                if (seat["seat"].ToString() == seatNumber)
                {
                    seat["reserved"] = "true";

                    // Serialize the updated JSON data
                    string updatedJson = JsonConvert.SerializeObject(auditoriumData, Formatting.Indented);

                    // Write the updated JSON data back to the file
                    try
                    {
                        File.WriteAllText(fileName, updatedJson);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating JSON file: {ex.Message}");
                    }

                    return;
                }
            }
        }
    }

    private static void SaveReservationToCSV(string username, string movieTitle, DateTime date, string auditoriumName, string seatNumber)
    {
        var movieInfo = MovieScheduleAccess.GetMovieSchedule().FirstOrDefault(m => m["movieTitle"].ToString() == movieTitle);
        if (movieInfo != null && DateTime.TryParseExact(movieInfo["displayTime"].ToString(), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime movieDisplayTime))
        {
            string reservationDetails = $"{username},{movieTitle},{movieDisplayTime:yyyy-MM-dd HH:mm},{auditoriumName},{seatNumber}";
            try
            {
                File.AppendAllText(Path.Combine(jsonFolderPath, "ReservationHistory.csv"), reservationDetails + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving reservation: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Movie not found in schedule or error parsing display time for movie: {movieTitle}");
        }
    }

    public static void MakeReservation(string username)
    {
        Console.Write("Enter date (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime selectedDate))
        {
            PrintMoviesForDay(selectedDate);

            Console.Write("Enter the movie title you want to reserve: ");
            string movieTitle = Console.ReadLine();

            var movieSchedule = GetMovieSchedule();
            var movieInfo = movieSchedule.FirstOrDefault(m => m["movieTitle"].ToString() == movieTitle);

            if (movieInfo != null)
            {
                string auditoriumFileName = movieInfo["filename"].ToString();
                DisplayAuditoriumFromFile(auditoriumFileName); // Display auditorium layout

                var auditoriumData = GetAuditoriumData(auditoriumFileName);

                if (auditoriumData != null)
                {
                    bool reservationSuccessful = false;
                    while (!reservationSuccessful)
                    {
                        Console.Write("Enter seat number(s) to reserve (separated by commas): ");
                        string[] seatNumbers = Console.ReadLine().Split(',');

                        bool allSeatsAvailable = true;
                        foreach (string seatNumber in seatNumbers)
                        {
                            if (!IsSeatAvailable(auditoriumData, seatNumber.Trim()))
                            {
                                Console.WriteLine($"Seat {seatNumber} is not available.");
                                allSeatsAvailable = false;
                                break;
                            }
                        }

                        if (allSeatsAvailable)
                        {
                            foreach (string seatNumber in seatNumbers)
                            {
                                ReserveSeatAndUpdateFile(auditoriumData, seatNumber.Trim(), Path.Combine(jsonFolderPath, auditoriumFileName));
                                SaveReservationToCSV(username, movieTitle, selectedDate, movieInfo["auditorium"].ToString(), seatNumber.Trim());
                            }
                            Console.WriteLine("Reservation successful!");
                            reservationSuccessful = true;
                        }
                        else
                        {
                            Console.WriteLine("Please choose different seat(s).");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error retrieving auditorium data.");
                }
            }
            else
            {
                Console.WriteLine("Movie not found in schedule.");
            }
        }
        else
        {
            Console.WriteLine("Invalid date format.");
        }
    }

    private static JObject GetAuditoriumData(string fileName)
    {
        try
        {
            string jsonContent = File.ReadAllText(Path.Combine(jsonFolderPath, fileName));
            return JsonConvert.DeserializeObject<JObject>(jsonContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading auditorium data: {ex.Message}");
            return null;
        }
    }

    private static void DisplayAuditoriumFromFile(string fileName)
    {
        AuditoriumsPresentation.DisplayAuditoriumFromFile(Path.Combine(jsonFolderPath, fileName));
    }
}
