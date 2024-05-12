
public static class ReservationHistory
{
    public static void DisplayReservationHistory(string username, List<Reservation> reservations)
    {
        if (reservations.Count > 0)
        {
            if (!string.IsNullOrEmpty(username))
            {
                Console.WriteLine($"Reservation History for {username}:");
            }
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Movie: {reservation.MovieTitle}, Date: {reservation.Date}, Auditorium: {reservation.Auditorium}, Seat: {reservation.SeatNumber}");
            }
        }
        else
        {
            Console.WriteLine("No reservations found for this user.");
        }
    }
}
