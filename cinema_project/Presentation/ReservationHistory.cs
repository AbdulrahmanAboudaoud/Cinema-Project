using System.Globalization;

public static class ReservationHistory
{
    public static void ViewReservationHistory(string username)
    {
        // Load reservation history for the user from CSV file
        List<Reservation> userReservations = LoadUserReservationsFromCSV(username);

        // Display reservation history to the user
        if (userReservations.Count > 0)
        {
            Console.WriteLine("Your Reservation History:");
            foreach (var reservation in userReservations)
            {
                Console.WriteLine($"Movie: {reservation.MovieTitle}, Date: {reservation.Date}, Auditorium: {reservation.Auditorium}, Seat: {reservation.SeatNumber}");
            }
        }
        else
        {
            Console.WriteLine("No reservations found.");
        }
    }

    private static List<Reservation> LoadUserReservationsFromCSV(string username)
    {
        List<Reservation> userReservations = new List<Reservation>();

        try
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines("C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\ReservationHistory.csv");

            foreach (string line in lines)
            {
                // Split the line by comma to extract reservation details
                string[] parts = line.Split(',');
                if (parts.Length == 4 && parts[0] == username)
                {
                    // Create a reservation object and add it to the list
                    userReservations.Add(new Reservation(
                    movieTitle: parts[1],
                    date: DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
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