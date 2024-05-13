using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;


public static class ReservationLogic
{

    public static void ViewReservationHistory(string username)
    {
        List<Reservation> userReservations = ReservationAccess.LoadReservationHistory(username);
        ReservationHistory.DisplayReservationHistory(username, userReservations);
    }



    private static string jsonFolderPath = @"C:\Users\Gebruiker\OneDrive - Hogeschool Rotterdam\Github\Cinema-Project\cinema_project\DataSources";
    //private static string jsonFolderPath = @"C:\Users\Joseph\Documents\GitHub\Cinema-Project\cinema_project\DataSources\";

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

    public static void MakeReservation(string username)
    {
        Console.Write("Enter date (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime selectedDate))
        {
            PrintMoviesForDay(selectedDate);

            Console.Write("Enter the movie title you want to reserve: ");
            string movieTitle = Console.ReadLine();

            var movieSchedule = MovieScheduleAccess.GetMovieSchedule();
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
                                ReservationAccess.SaveReservationToCSV(username, movieTitle, selectedDate, movieInfo["auditorium"].ToString(), seatNumber.Trim());
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

    public static void CancelReservation(string username)
    {
        // Load reservation history for the user
        List<Reservation> userReservations = ReservationAccess.LoadReservationHistory(username);

        if (userReservations.Count > 0)
        {
            Console.WriteLine("Select a reservation to cancel:");
            for (int i = 0; i < userReservations.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userReservations[i].MovieTitle} - {userReservations[i].Date} - {userReservations[i].Auditorium} - {userReservations[i].SeatNumber}");
            }

            Console.Write("Enter the number of the reservation to cancel: ");
            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= userReservations.Count)
            {
                Reservation reservationToCancel = userReservations[selection - 1];
                // Remove the reservation from the CSV file
                ReservationAccess.RemoveReservationFromCSV(username, reservationToCancel);

                // Update auditorium layout file if necessary
                UpdateAuditoriumLayoutFile(reservationToCancel.MovieTitle, reservationToCancel.Date, reservationToCancel.Auditorium, reservationToCancel.SeatNumber, false);

                Console.WriteLine("Reservation canceled successfully.");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }
        else
        {
            Console.WriteLine("No reservations found for this user.");
        }
    }


    public static void EditReservation(string username)
    {
        // Load reservation history for the user
        List<Reservation> userReservations = ReservationAccess.LoadReservationHistory(username);

        if (userReservations.Count > 0)
        {
            Console.WriteLine("Select a reservation to edit:");
            for (int i = 0; i < userReservations.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userReservations[i].MovieTitle} - {userReservations[i].Date} - {userReservations[i].Auditorium} - {userReservations[i].SeatNumber}");
            }

            Console.Write("Enter the number of the reservation to edit: ");
            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= userReservations.Count)
            {
                Reservation reservationToEdit = userReservations[selection - 1];
                // Remove the original reservation from the CSV file
                ReservationAccess.RemoveReservationFromCSV(username, reservationToEdit);

                // Prompt the user to choose a new seat
                Console.Write("Enter new seat number: ");
                string newSeatNumber = Console.ReadLine();

                // Update reservation details
                string oldSeatNumber = reservationToEdit.SeatNumber; // Store the old seat number
                reservationToEdit.SeatNumber = newSeatNumber;

                // Add the edited reservation back to the CSV file
                ReservationAccess.SaveReservationToCSV(username, reservationToEdit.MovieTitle, reservationToEdit.Date, reservationToEdit.Auditorium, reservationToEdit.SeatNumber); // Provide the reservation date

                // Update auditorium layout file if necessary
                UpdateAuditoriumLayoutFile(reservationToEdit.MovieTitle, reservationToEdit.Date, reservationToEdit.Auditorium, oldSeatNumber, reservationToEdit.SeatNumber, true);

                Console.WriteLine("Reservation edited successfully.");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }
        else
        {
            Console.WriteLine("No reservations found for this user.");
        }
    }




    // Helper method to update the auditorium layout file
    private static void UpdateAuditoriumLayoutFile(string movieTitle, DateTime displayDate, string auditorium, string seatNumber, bool reserved)
    {
        try
        {
            string fileName = $"{movieTitle}-{displayDate:yyyyMMdd-HHmm}-{auditorium}.json";
            string filePath = Path.Combine(jsonFolderPath, fileName);

            string jsonContent = File.ReadAllText(filePath);
            var auditoriumData = JsonConvert.DeserializeObject<JObject>(jsonContent);

            var seatToUpdate = auditoriumData["auditoriums"][0]["layout"]
                .SelectMany(row => row)
                .FirstOrDefault(seat => seat["seat"].ToString() == seatNumber);

            if (seatToUpdate != null)
            {
                seatToUpdate["reserved"] = reserved.ToString().ToLower();
                File.WriteAllText(filePath, JsonConvert.SerializeObject(auditoriumData));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating auditorium layout file: {ex.Message}");
        }
    }

    private static void UpdateAuditoriumLayoutFile(string movieTitle, DateTime displayDate, string auditoriumName, string oldSeatNumber, string newSeatNumber, bool reserved)
    {
        try
        {
            string fileName = $"{movieTitle}-{displayDate:yyyyMMdd-HHmm}-{auditoriumName}.json";
            string filePath = Path.Combine(jsonFolderPath, fileName);

            string jsonContent = File.ReadAllText(filePath);
            var auditoriumData = JsonConvert.DeserializeObject<JObject>(jsonContent);

            // Find and update the old seat
            var oldSeatToUpdate = auditoriumData["auditoriums"][0]["layout"]
                .SelectMany(row => row)
                .FirstOrDefault(seat => seat["seat"].ToString() == oldSeatNumber);

            if (oldSeatToUpdate != null)
            {
                oldSeatToUpdate["reserved"] = "false"; // Mark the old seat as not reserved
            }

            // Find and update the new seat
            var newSeatToUpdate = auditoriumData["auditoriums"][0]["layout"]
                .SelectMany(row => row)
                .FirstOrDefault(seat => seat["seat"].ToString() == newSeatNumber);

            if (newSeatToUpdate != null)
            {
                newSeatToUpdate["reserved"] = "true"; // Mark the new seat as reserved
            }

            // Serialize the updated JSON data
            string updatedJson = JsonConvert.SerializeObject(auditoriumData, Formatting.Indented);

            // Write the updated JSON data back to the file
            File.WriteAllText(filePath, updatedJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating auditorium layout file: {ex.Message}");
        }
    }




}
