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
                DisplayAuditoriumFromFile(auditoriumFileName); 

                var auditoriumData = AuditoriumsDataAccess.GetAuditoriumData(auditoriumFileName);

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
                                AuditoriumsDataAccess.ReserveSeatAndUpdateFile(auditoriumData, seatNumber.Trim(), Path.Combine(AuditoriumsDataAccess.jsonFolderPath, auditoriumFileName));
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

    

    private static void DisplayAuditoriumFromFile(string fileName)
    {
        AuditoriumsPresentation.DisplayAuditoriumFromFile(Path.Combine(AuditoriumsDataAccess.jsonFolderPath, fileName));
    }

    public static void CancelReservation(string username)
    {
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
                ReservationAccess.RemoveReservationFromCSV(username, reservationToCancel);
                AuditoriumsDataAccess.UpdateAuditoriumLayoutFile(reservationToCancel.MovieTitle, reservationToCancel.Date, reservationToCancel.Auditorium, reservationToCancel.SeatNumber, false);

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
                ReservationAccess.RemoveReservationFromCSV(username, reservationToEdit);

                Console.Write("Enter new seat number: ");
                string newSeatNumber = Console.ReadLine();

                string oldSeatNumber = reservationToEdit.SeatNumber; 
                reservationToEdit.SeatNumber = newSeatNumber;

                ReservationAccess.SaveReservationToCSV(username, reservationToEdit.MovieTitle, reservationToEdit.Date, reservationToEdit.Auditorium, reservationToEdit.SeatNumber); // Provide the reservation date

                AuditoriumsDataAccess.UpdateAuditoriumLayoutFile(reservationToEdit.MovieTitle, reservationToEdit.Date, reservationToEdit.Auditorium, oldSeatNumber, reservationToEdit.SeatNumber, true);

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
}
