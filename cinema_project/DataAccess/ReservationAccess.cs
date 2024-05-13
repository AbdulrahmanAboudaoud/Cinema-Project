using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;

public static class ReservationAccess
{
    private const string reservationFilePath = @"C:\Users\Gebruiker\OneDrive - Hogeschool Rotterdam\Github\Cinema-Project\cinema_project\DataSources\ReservationHistory.csv";
    private static string jsonFolderPath = @"C:\Users\Gebruiker\OneDrive - Hogeschool Rotterdam\Github\Cinema-Project\cinema_project\DataSources";

    public static List<Reservation> LoadReservationHistory(string username)
    {
        List<Reservation> userReservations = new List<Reservation>();
        try
        {
            string[] lines = File.ReadAllLines(reservationFilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 5 && parts[0] == username)
                {
                    DateTime date;
                    if (DateTime.TryParseExact(parts[2], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        userReservations.Add(new Reservation(
                            username: parts[0],
                            movieTitle: parts[1],
                            date: date,
                            auditorium: parts[3],
                            seatNumber: parts[4]
                        ));
                    }
                    else
                    {
                        Console.WriteLine($"Error parsing date for reservation: {parts[1]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading reservation history: {ex.Message}");
        }
        return userReservations;
    }

    public static List<Reservation> LoadAllReservations()
    {
        List<Reservation> userReservations = new List<Reservation>();

        try
        {
            string[] lines = File.ReadAllLines(reservationFilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                    DateTime date;
                    if (DateTime.TryParseExact(parts[2], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        userReservations.Add(new Reservation(
                        username: parts[0],
                        movieTitle: parts[1],
                        date: date,
                        auditorium: parts[3],
                        seatNumber: parts[4]
                        ));
                    }  
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading reservation history: {ex.Message}");
        }

        return userReservations;
    }

    public static void SaveReservationToCSV(string username, string movieTitle, DateTime date, string auditoriumName, string seatNumber)
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

    public static void RemoveReservationFromCSV(string username, Reservation reservationToRemove)
    {
        string[] lines = File.ReadAllLines(Path.Combine(jsonFolderPath, "ReservationHistory.csv"));

        string reservationDetails = $"{username},{reservationToRemove.MovieTitle},{reservationToRemove.Date:yyyy-MM-dd HH:mm},{reservationToRemove.Auditorium},{reservationToRemove.SeatNumber}";
        var newLines = lines.Where(line => line != reservationDetails).ToArray();

        File.WriteAllLines(Path.Combine(jsonFolderPath, "ReservationHistory.csv"), newLines);
    }
}
