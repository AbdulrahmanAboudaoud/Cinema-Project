using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;

public static class ReservationAccess
{
    private const string reservationFilePath = @"C:\Users\abdul\OneDrive\Documents\GitHub\Cinema-Project\cinema_project\DataSources\ReservationHistory.csv";

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
}
